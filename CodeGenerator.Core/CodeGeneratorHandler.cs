using CodeGenerator.Core.Dtos;
using Database.Data.Entities;
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
            
            File.WriteAllText(filePath, RemoveComment(generatedCode));
        }

        public static List<string> GetClassMethodNames(string path)
        {
            var rootNode = GetRootNode(path);

            var methods = rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>();

            return methods.Select(x=>x.Identifier.Text).ToList();
        }

        public static List<string> MethodParameters(string path,string methodName)
        {
            var rootNode = GetRootNode(path);

            var method = rootNode.DescendantNodes().OfType<MethodDeclarationSyntax>().FirstOrDefault(x=>x.Identifier.ValueText==methodName);

            return method.ParameterList.Parameters.Select(x => x.Identifier.Text).ToList();
        }

        public static List<ClassPropertyModel> GetClassProperties(string path)
        {           

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);            

            var properties = rootNode.DescendantNodes().OfType<PropertyDeclarationSyntax>();
            var properties2 = rootNode.DescendantNodes().OfType<FieldDeclarationSyntax>();

            var fields = new List<ClassPropertyModel>();

            foreach (var property in properties)
            {
                ClassPropertyModel field = new ClassPropertyModel();

                var typeString = property.Type.ToString() switch
                {
                    "string" => "System.String",
                    "int" => "System.Int32",                   
                    _ => property.Type.ToString()

                };

                field.Type = typeString;
                var typeParamsOrNull = (property.Type as GenericNameSyntax)?.TypeArgumentList.Arguments;
                field.Name = property.Identifier.ValueText;

                fields.Add(field);
            }

            foreach (var property in properties2)
            {
                ClassPropertyModel field = new ClassPropertyModel();

                var typeString = property.Declaration.Type.ToString() switch
                {
                    "string" => "System.String",
                    "int" => "System.Int32",
                    _ => property.Declaration.Type.ToString()

                };

                field.Type = typeString;
               
                field.Name = property.Declaration.Variables.First().Identifier.ValueText;

                fields.Add(field);
            }


            return fields;
        }

        public static List<EnumField> GetEnumFields(string path)
        {
            var fields= new List<EnumField>();
            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);           

            var enumDeclarations = rootNode.DescendantNodes().OfType<EnumDeclarationSyntax>();

            foreach (var enumDeclaration in enumDeclarations)
            {
                var enumFields = enumDeclaration.Members.OfType<EnumMemberDeclarationSyntax>();

                foreach (var enumField in enumFields)
                {
                    var fieldName = enumField.Identifier.ValueText;
                    
                    int.TryParse(enumField.EqualsValue?.Value.ToString() , out int fieldValue);

                    int? value = enumField.EqualsValue?.Value.ToString() != null ? fieldValue : null;

                    string? name = null;
                    int? order = null;
                    

                    var attributeList = enumField.AttributeLists.FirstOrDefault();
                    var displayAttribute = attributeList?.Attributes.FirstOrDefault(a => a.Name.ToString() == "Display");
                    if (displayAttribute != null)
                    {                       
                         name = displayAttribute.ArgumentList.Arguments.FirstOrDefault(a => a.NameEquals.Name.Identifier.ValueText == "Name")?.Expression.ToString();
                        var orderArgument = displayAttribute.ArgumentList.Arguments.LastOrDefault(a => a.NameEquals.Name.Identifier.ValueText == "Order");
                        var  orderString = orderArgument?.Expression.ToString();
                        int.TryParse(orderString, out int orderValue);

                        order = orderString != null ? orderValue : null;
                    }

                    fields.Add(new EnumField
                    {
                        Name = fieldName,
                        Value = value,
                        Description =name,
                        Order = order,
                    });

                }
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

        public static string? GetClassName(string path)
        {

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);

            var classes = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();

            var className=classes.FirstOrDefault()?.Identifier.ValueText;

            return className;
        }

        public static string? GetEnumName(string path)
        {

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);

            var classes = rootNode.DescendantNodes().OfType<EnumDeclarationSyntax>();

            var className = classes.FirstOrDefault()?.Identifier.ValueText;

            return className;
        }

        public static SyntaxKind? GetClassType(string path)
        {

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);

            var classes = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
            if (!classes.Any())
            {
                return SyntaxKind.None;
            }
            if (classes.FirstOrDefault().Keyword.IsKind(SyntaxKind.ClassKeyword))
            {
                return SyntaxKind.ClassKeyword;
            }
            if (classes.FirstOrDefault().Keyword.IsKind(SyntaxKind.InterfaceKeyword))
            {
                return SyntaxKind.InterfaceKeyword;

            }
            if (classes.FirstOrDefault().Keyword.IsKind(SyntaxKind.EnumKeyword))
            {
                return SyntaxKind.EnumKeyword;
            }  

            return SyntaxKind.None;
        }

        public static SyntaxNode? GetEnumType(string path)
        {

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);

           return rootNode
             .DescendantNodes()
             .OfType<EnumDeclarationSyntax>()
             .SingleOrDefault();


            
        }

        public static string? GetClassBaseTypeName(string path)
        {

            // Get the root node of the syntax tree
            var rootNode = GetRootNode(path);
            var classType = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            var baseType = classType.BaseList.Types.FirstOrDefault();
            var nameOfFirstBaseType = baseType.Type.ToString();
            return nameOfFirstBaseType;
        }

        public static string RemoveComment(string code)
        {
            string multilineString = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

";
            string modifiedContent = code.Replace(multilineString, "");

            return modifiedContent;
        }


    }
}
