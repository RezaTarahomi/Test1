using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;

namespace CodeGenerator.Test.Unit
{
    public class ApiGenerator
    {
        public static void Generate(string controllerPath, string entityName, string apiType)
        {
            // Create a syntax tree from an existing C# file
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(controllerPath));

            // Get the root node of the syntax tree
            var rootNode = syntaxTree.GetRoot();

            // Create a new syntax tree for the modified code
            SyntaxTree modifiedSyntaxTree = CSharpSyntaxTree.Create((CSharpSyntaxNode)rootNode);

            // Get the compilation unit root
            var compilationUnit = (CompilationUnitSyntax)modifiedSyntaxTree.GetRoot();

            
            // Create a new method for the POST API
            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.ParseTypeName("IActionResult"), "Get"+entityName +"s54")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))

                .AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("HttpGet")))))
                  .WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ParseStatement("return Ok();")
                        
                    ));

            var namespaceDeclaration = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            var classDeclaration = namespaceDeclaration.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            var updatedClassDeclaration = classDeclaration.AddMembers(methodDeclaration);

            var updatedSyntaxTree = syntaxTree.WithRootAndOptions(rootNode, syntaxTree.Options);

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();
           

            File.WriteAllText(controllerPath, updatedRoot.ToString());
        }

        private static CodeMemberMethod GenerateHttpGetMethod()
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            method.Name = "Get";           

            // Add the [HttpGet] attribute to the method
            method.CustomAttributes.Add(new CodeAttributeDeclaration(
                new CodeTypeReference("HttpGetAttribute")
            ));

            // Add the method body to return an example response
            method.Statements.Add(new CodeMethodReturnStatement(
                new CodeObjectCreateExpression("Response", new CodePrimitiveExpression("Sample response"))
            ));

            return method;
        }

        private static CodeCompileUnit GenerateCodeCompileUnit(string controllerPath)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(controllerPath));

            // Get the root node of the syntax tree
            var rootNode = syntaxTree.GetRoot();

            var namespaceDeclaration = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            var classDeclaration = namespaceDeclaration.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            return new CodeCompileUnit();
        }
    }
}
