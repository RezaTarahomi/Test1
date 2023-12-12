using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

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
                updatedClassDeclaration= classDeclaration.AddMembers(item);
            }


            var updatedSyntaxTree = syntaxTree.WithRootAndOptions(rootNode, syntaxTree.Options);

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();


            File.WriteAllText(controllerPath, updatedRoot.ToString());
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

        GetAllGrid = 1,
        GetById = 2,
        GetList = 3,
        Create = 4,
        Edit = 5,
        DeleteById = 6,
        DeleteByIds = 7,
        ActiveDeactive = 8,

        Get = 9,
        Post = 10,
        Patch = 11,

    }


}
