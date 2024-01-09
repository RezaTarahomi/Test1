using CodeGenerator.Core.Dtos;
using CodeGenerator.Core.Extensions;
using Database.Data.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CodeGenerator.Core
{
    public class ApiGenerator
    {
        public static void Generate(string controllerPath, List<CreateApiMethodModel> apiModels)
        {
            // Create a syntax tree from an existing C# file
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(controllerPath));

            // Get the root node of the syntax tree
            var rootNode = syntaxTree.GetRoot();

            // Create a new syntax tree for the modified code
            SyntaxTree modifiedSyntaxTree = CSharpSyntaxTree.Create((CSharpSyntaxNode)rootNode);

            // Get the compilation unit root
            var compilationUnit = (CompilationUnitSyntax)modifiedSyntaxTree.GetRoot();

            var namespaceDeclaration = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            var classDeclaration = namespaceDeclaration.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            var apies = GenerateApiMethods(apiModels);

            ClassDeclarationSyntax updatedClassDeclaration = null;

            foreach (var item in apies)
            {
                updatedClassDeclaration = classDeclaration.AddMembers(item);            }


            var updatedSyntaxTree = syntaxTree.WithRootAndOptions(rootNode, syntaxTree.Options);

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();

            File.WriteAllText(controllerPath, CodeGeneratorHandler.RemoveComment(updatedRoot.ToString()));
        }

        public static void GenerateRequest(string domainName,
            string entityName,
            ApiType apiType,
            EntityCreateModel? request = null,
            EntityCreateModel? responseModel = null,
            string? apiName = null)
        {
            string requestTye = GetRequestType(apiType);
            string? requestName = GetRequestName(apiType, entityName, apiName);

            var projectName = ProjectStructure.Application;

            var nameSpacePath = Path.Combine(
                projectName, "Features",
                domainName,
                entityName.GetPluralForm(), requestTye.GetPluralForm(),
                requestName);

            var path = Path.Combine(DirectoryHandler.GetAppRoot(), nameSpacePath);


            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace blankNamespaces = new CodeNamespace();

            var entityUsing = ProjectStructure.Application + "." + ProjectStructure.Id_CaptionPath;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(entityUsing));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("System.Linq"));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("MediatR"));
            compileUnit.Namespaces.Add(blankNamespaces);

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(nameSpacePath.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);

            CodeTypeDeclaration newClass = new CodeTypeDeclaration(requestName + requestTye);
            newClass.IsClass = false;
            newClass.TypeAttributes = TypeAttributes.Public;


            string? responseType = responseModel?.Name;

            if (responseModel != null && responseModel.Properties.Any())
            {
                var responseModelName = GetResponseModelName(apiType, entityName, responseModel?.Name);
                responseModel.Name = responseModelName;
                EntityGenerator.GenerateEntity(nameSpacePath, responseModel);
                responseType = GetResponseType(apiType, responseModelName);
            }

            var basseType = responseType != null ? $"IRequest<{responseType}>" : "IRequest";

            newClass.BaseTypes.Add(new CodeTypeReference(basseType));

            if (request != null)
            {
                foreach (var property in request.Properties)
                {
                    CodeMemberField idField = new CodeMemberField(property.Type, property.Name);
                    idField.Attributes = property.MemberAttributes ?? MemberAttributes.Public;
                    idField.Name += " { get; set; }//";

                    newClass.Members.Add(idField);
                }
            }

            // Add the class to the namespace
            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, requestName + requestTye, path);

        }

        private static string? GetResponseModelName(ApiType apiType, string? entityName, string? modelName)
        {
            modelName = apiType switch
            {
                ApiType.GetForGrid => $"{entityName}GridModel",
                ApiType.GetById => $"{entityName}Model",
                ApiType.GetList => $"Id_Caption",
                ApiType.Create => "Create" + entityName + "Model",
                ApiType.Edit => "Edit" + entityName + "Model",
                ApiType.DeleteById => null,
                ApiType.DeleteByIds => null,
                ApiType.Active => null,
                ApiType.Deactive => null,
                ApiType.Get => modelName,
                ApiType.Post => modelName,
                ApiType.Patch => modelName,
                _ => throw new NotImplementedException(),
            };


            return modelName;
        }

        private static string? GetResponseType(ApiType apiType, string? modelName)
        {
            var responseType = apiType switch
            {
                ApiType.GetForGrid => $"IQueryable<{modelName}>",
                ApiType.GetById => modelName,
                ApiType.GetList => $"List<{modelName}>",
                ApiType.Create => modelName,
                ApiType.Edit => modelName,
                ApiType.DeleteById => null,
                ApiType.DeleteByIds => null,
                ApiType.Active => null,
                ApiType.Deactive => null,
                ApiType.Get => modelName,
                ApiType.Post => modelName,
                ApiType.Patch => modelName,
                _ => throw new NotImplementedException(),
            };


            return responseType;
        }

        public static string? GetRequestName(ApiType apiType, string entityName, string? apiName)
        {
            return apiType switch
            {
                ApiType.GetForGrid => $"Get{entityName.GetPluralForm()}ForGrid",
                ApiType.GetById => $"Get{entityName}ById",
                ApiType.GetList => $"Get{entityName.GetPluralForm()}List",
                ApiType.Create => "Create" + entityName,
                ApiType.Edit => "Edit" + entityName,
                ApiType.DeleteById => $"Delete{entityName}ById",
                ApiType.DeleteByIds => $"Delete{entityName}ByIds",
                ApiType.Active => "Active" + entityName,
                ApiType.Deactive => "Deactive" + entityName,
                ApiType.Get => apiName,
                ApiType.Post => apiName,
                ApiType.Patch => apiName,
                _ => throw new NotImplementedException(),
            };
        }

        public static void GenerateHandler(string domainName, string entityName, ApiType apiType, EntityCreateModel? responseModel = null)
        {
            string requestTye = GetRequestType(apiType);
            string? requestName = GetRequestName(apiType, entityName, null);

            string repositoryType = requestTye == "Command" ? "Repository" : "Query";

            var projectName = ProjectStructure.Application;

            var nameSpacePath = Path.Combine(
                projectName, "Features",
                domainName,
                entityName.GetPluralForm(), requestTye.GetPluralForm(),
                requestName);

            var path = Path.Combine(DirectoryHandler.GetAppRoot(), nameSpacePath);


            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace blankNamespaces = new CodeNamespace();

            var entityUsing = ProjectStructure.Application + "." + ProjectStructure.Id_CaptionPath;
            var repositoryUsing = ProjectStructure.Application + "." + domainName;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(entityUsing));
            blankNamespaces.Imports.Add(new CodeNamespaceImport(repositoryUsing));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("System.Linq"));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("MediatR"));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("MediatR"));
            compileUnit.Namespaces.Add(blankNamespaces);

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(nameSpacePath.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);

            CodeTypeDeclaration newClass = new CodeTypeDeclaration(requestName + requestTye + "Handler");
            newClass.IsClass = false;
            newClass.TypeAttributes = TypeAttributes.Public;

            string? responseType = responseModel?.Name;

            if (responseModel != null)
            {
                var responseModelName = GetResponseModelName(apiType, entityName, responseModel?.Name);
                responseModel.Name = responseModelName;              
                responseType = GetResponseType(apiType, responseModelName);
            }          


            var basseType = responseType != null ? $"IRequestHandler<{requestName + requestTye},{responseType}>" : $"IRequestHandler<{requestName + requestTye}>";

            newClass.BaseTypes.Add(new CodeTypeReference(basseType));

            CodeMemberField idField = new CodeMemberField($"I{entityName}{repositoryType}", $"_{entityName.ToLower()}{repositoryType}");
            idField.Attributes = MemberAttributes.Private;
           // idField.UserData              

            newClass.Members.Add(idField);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression($"I{entityName}{repositoryType}", $"{entityName.ToLower()}{repositoryType}"));
            constructor.Statements.Add(new CodeSnippetExpression($"_{entityName.ToLower()}{repositoryType} = {entityName.ToLower()}{repositoryType} "));
            newClass.Members.Add(constructor);

            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = "Handle";
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;    
            
            method.ReturnType =new CodeTypeReference(responseType != null ?$"async Task<{responseType}>": "async Task<Unit>");
            method.Parameters.Add(new CodeParameterDeclarationExpression(requestName + requestTye, "request"));
            method.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken ", "cancellationToken"));
            method.Statements.Add(new CodeSnippetExpression("throw new NotImplementedException();"));
            newClass.Members.Add(method);

            // Add the class to the namespace
            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, requestName + requestTye + "Handler", path);
        }

        public static string GetRequestType(ApiType apiType)
        {
            var commansApiType = new List<ApiType>
            {
                ApiType.Create,
                ApiType.Edit,
                ApiType.DeleteById,
                ApiType.DeleteByIds,
                ApiType.Active,
                ApiType.Deactive,
                ApiType.Post,
                ApiType.Patch
            };
            if (commansApiType.Contains(apiType))
                return "Command";
            return "Query";


        }

        private static List<MethodDeclarationSyntax> GenerateApiMethods(List<CreateApiMethodModel> Apies)
        {
            var methods = new List<MethodDeclarationSyntax>();

            foreach (var api in Apies)
            {
                var method = GenerateApiMethod(api);

                methods.Add(GenerateMethod(method));
            };

            return methods;

        }

        private static CreateMethodModel GenerateApiMethod(CreateApiMethodModel Api)
        {
            var method = new CreateMethodModel();

            var mediatorRequestName = string.Empty;
            var apiAttribute = string.Empty;
            var hasRoute = false;
            var routeParameter = string.Empty;

            method.Modifires.Add(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            method.Modifires.Add(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));



            if (Api.Type == ApiType.GetList)
            {
                apiAttribute = "HttpGet";
                mediatorRequestName = $"Get{Api.EntityName}List";
                method.Name = Api.EntityName.GetPluralForm();
                hasRoute = true;
                routeParameter = @"( ""List"" )";
            }

            if (hasRoute)
            {
                var routeAttribute = SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Attribute(
                        SyntaxFactory.ParseName("Route"))
                          .WithArgumentList(SyntaxFactory.ParseAttributeArgumentList(routeParameter))));

                method.Attributes.Add(routeAttribute);

            }

            var apiTypeAttribute = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(apiAttribute))));

            method.Attributes.Add(apiTypeAttribute);

            method.Body.Add(SyntaxFactory.ParseStatement($"var data = await _mediator.Send(new {mediatorRequestName}Query());"));
            method.Body.Add(SyntaxFactory.ParseStatement("return Ok(data);"));

            return method;

        }
        private static MethodDeclarationSyntax GenerateMethod(CreateMethodModel model)
        {           

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                 SyntaxFactory.ParseTypeName("Task<IActionResult>"), model.Name)
                .AddModifiers(model.Modifires.ToArray())
                .AddAttributeLists(model.Attributes.ToArray())
                .WithBody(SyntaxFactory.Block(model.Body.ToArray()));

            return methodDeclaration;
        }

    }


    public class CreateMethodModel
    {
        public CreateMethodModel()
        {
            Attributes = new List<AttributeListSyntax>();
            Modifires = new List<SyntaxToken>();
            Body = new List<StatementSyntax>();
        }

        public List<AttributeListSyntax> Attributes { get; set; }
        public string Name { get; set; }
        public List<SyntaxToken> Modifires { get; set; }
        public List<StatementSyntax> Body { get; set; }
    }

    public class CreateApiMethodModel
    {
        public ApiType Type { get; set; }
        public string Name { get; set; }
        public string EntityName { get; set; }

    }


}
