using CodeGenerator.Core.Dtos;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core
{
    public class CodeGeneratorHandler
    {
        public static void GenerateCSharpClassFile(CodeCompileUnit compileUnit,string fileName, string path)
        {
            // Generate the C# code
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            string generatedCode = string.Empty;

            using (StringWriter writer = new StringWriter())
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
                generatedCode = writer.ToString();
            }

            var directory = Path.Combine(DirectoryHandler.GetAppRoot(), path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save the generated code to a file
            string filePath = Path.Combine(directory, fileName + ".cs");
            File.WriteAllText(filePath, generatedCode);
        }

        public static List<string> GetClassMethodNames(string path)
        {
            var rootNode = GetRootNode(path);

            var methods = rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>();

            return methods.Select(x=>x.Identifier.Text).ToList();
        }

        public static List<ClassPropertyModel> GetClassProperties(string path)
        {           

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);            

            var properties = rootNode.DescendantNodes().OfType<PropertyDeclarationSyntax>();

            var fields = new List<ClassPropertyModel>();

            foreach (var property in properties)
            {
                ClassPropertyModel field = new ClassPropertyModel();

                var typeString = property.Type.ToString() switch
                {
                    "string" => "System.String",
                    "int" => "System.Int32",
                    _ => throw new NotImplementedException()

                };

                field.Type = Type.GetType(typeString);
                var typeParamsOrNull = (property.Type as GenericNameSyntax)?.TypeArgumentList.Arguments;
                field.Name = property.Identifier.ValueText;

                fields.Add(field);
            }


            return fields;
        }

        private static SyntaxNode GetRootNode(string path)
        {
            // Create a syntax tree from an existing C# file
            path = Path.Combine(DirectoryHandler.GetAppRoot(), path);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));

            // Get the root node of the syntax tree
            return syntaxTree.GetRoot();
        }
    }
}
