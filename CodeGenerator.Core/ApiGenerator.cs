using CodeGenerator.Core.Dtos;
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
                updatedClassDeclaration = classDeclaration.AddMembers(item);
            }


            var updatedSyntaxTree = syntaxTree.WithRootAndOptions(rootNode, syntaxTree.Options);

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();


            File.WriteAllText(controllerPath, updatedRoot.ToString());
        }

        public static void GenerateRequest(string domainName,
            string entityName,
            ApiType apiType,
            EntityCreateModel? request=null,
            EntityCreateModel? response =null,
            string? apiName=null )
        {
            string requestTye = GetRequestType(apiType);
            string? requestName = GetRequestName(apiType,entityName,apiName);
            var responseType = GetResponseType(apiType,entityName,response?.Name);

           

            var projectName = ProjectStructure.Application;            

            var nameSpacePath = Path.Combine(
                projectName, "Features",
                domainName,
                DirectoryHandler.GetPluralForm(entityName), DirectoryHandler.GetPluralForm(requestTye),
                requestName);

            var path = Path.Combine(DirectoryHandler.GetAppRoot(), nameSpacePath);

            if (response!=null &&  response.Properties.Any())
            {
                response.Name = entityName+"Model";
                EntityGenerator.GenerateEntity(path, response);
            }

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

            var basseType = responseType != null ?  $"IRequest<{responseType}>" : "IRequest";

            newClass.BaseTypes.Add(new CodeTypeReference(basseType));

            foreach (var property in request.Properties)
            {
                CodeMemberField idField = new CodeMemberField(property.Type, property.Name);
                idField.Attributes = property.MemberAttributes ?? MemberAttributes.Public;
                idField.Name += " { get; set; }//";

                newClass.Members.Add(idField);
            }

            // Add the class to the namespace
            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, requestName + requestTye, path);

        }

        private static string? GetResponseType(ApiType apiType, string entityName, string? responseType)
        {
             responseType = apiType switch
            {
                ApiType.GetForGrid => $"Iqueryable<{entityName}>",
                ApiType.GetById => $"{entityName}Model",
                ApiType.GetList => $"List<Id_Caption>",
                ApiType.Create => "Create" + entityName +"Model",
                ApiType.Edit => "Edit" + entityName +"Model",
                ApiType.DeleteById => null,
                ApiType.DeleteByIds => null,
                ApiType.Active => null,
                ApiType.Deactive => null,
                ApiType.Get => responseType,
                ApiType.Post => responseType,
                ApiType.Patch => responseType,
                _ => throw new NotImplementedException(),
            };

            
            return responseType ;
        }

        private static string? GetRequestName(ApiType apiType, string entityName, string? apiName)
        {
           return apiType switch
           {
               ApiType.GetForGrid => $"Get{DirectoryHandler.GetPluralForm(entityName)}ForGrid",
               ApiType.GetById => $"Get{entityName}ById",
               ApiType.GetList => $"Get{DirectoryHandler.GetPluralForm(entityName)}List",
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

        private static string GetRequestType(ApiType apiType)
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
                method.Name = DirectoryHandler.GetPluralForm(Api.EntityName);
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
            var a = model.Modifires.ToArray();
            var b = model.Attributes.ToArray();
            var c = model.Body.ToArray();

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

    public enum ApiType : byte
    {

        GetForGrid = 1,
        GetById = 2,
        GetList = 3,
        Create = 4,
        Edit = 5,
        DeleteById = 6,
        DeleteByIds = 7,
        Active = 8,
        Deactive = 9,

        Get = 10,
        Post = 11,
        Patch = 12,

    }


}
