using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Core.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerator.Core
{
    public class EntityGenerator
    {


        public static void GenerateEntity(string path, EntityCreateModel model)
        {
            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(path.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);

            CodeTypeDeclaration newClass = new CodeTypeDeclaration(model.Name);
            newClass.IsClass = true;
            newClass.TypeAttributes = TypeAttributes.Public;

            foreach (var property in model.Properties)
            {
                CodeMemberField idField = new CodeMemberField(property.Type, property.Name);
                idField.Attributes = property.MemberAttributes ?? MemberAttributes.Public;
                idField.Name += " { get; set; }//";

                newClass.Members.Add(idField);
            }

            // Add the class to the namespace
            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, model.Name, path);

        }


        private void GenerateVehicleClass()
        {
            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace("Models");
            compileUnit.Namespaces.Add(codeNamespace);

            // Create the Vehicle class
            CodeTypeDeclaration vehicleClass = new CodeTypeDeclaration("VehicleUserType");
            vehicleClass.IsClass = true;
            vehicleClass.TypeAttributes = TypeAttributes.Public;

            // Add the Id property
            CodeMemberField idField = new CodeMemberField(typeof(int), "Id");
            vehicleClass.Members.Add(idField);

            // Add the Model property
            CodeMemberField modelField = new CodeMemberField(typeof(string), "Models");
            vehicleClass.Members.Add(modelField);

            // Add the class to the namespace
            codeNamespace.Types.Add(vehicleClass);

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

            // Save the generated code to a file
            string filePath = Path.Combine("Models", "VehicleUserType.cs");
            File.WriteAllText(filePath, generatedCode);

        }

        private void GenerateVehicleController()
        {
            // Create a syntax tree from an existing C# file
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText("Api\\VehicleController.cs"));

            // Get the root node of the syntax tree
            var rootNode = syntaxTree.GetRoot();

            // Create a new syntax tree for the modified code
            SyntaxTree modifiedSyntaxTree = CSharpSyntaxTree.Create((CSharpSyntaxNode)rootNode);

            // Get the compilation unit root
            var compilationUnit = (CompilationUnitSyntax)modifiedSyntaxTree.GetRoot();

            // Add using directive for Model namespace
            var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Models"));
            compilationUnit = compilationUnit.AddUsings(usingDirective);

            // Create a new method for the POST API
            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.ParseTypeName("void"), "CreateVehicle")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("vehicle"))
                        .WithType(SyntaxFactory.ParseTypeName("Vehicle")))
                .WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ParseStatement("Console.WriteLine(\"Creating a new vehicle...\");"),
                        SyntaxFactory.ParseStatement("// Add logic to create the vehicle")
                    ));

            var namespaceDeclaration = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            var classDeclaration = namespaceDeclaration.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            var updatedClassDeclaration = classDeclaration.AddMembers(methodDeclaration);

            var updatedSyntaxTree = syntaxTree.WithRootAndOptions(rootNode, syntaxTree.Options);

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();

            var tree = CSharpSyntaxTree.Create(updatedClassDeclaration.NormalizeWhitespace());

            File.WriteAllText("Api\\VehicleController.cs", tree.ToString());
        }

        public static void CallInjectedClassMethod(string baseClassPath, string baseClassMethodName, string injectedClass, MethodSignModel injectedClassMethod)
        {
            throw new NotImplementedException();
        }

        public static void Inject(string classPath, string injectedClass)
        {
            
            var classFields = CodeGeneratorHandler.GetClassProperties(classPath).Select(x => x.Name).ToList();
            var injectedClassName = Path.GetFileNameWithoutExtension(injectedClass);
             injectedClassName = injectedClassName.Substring(1);

            if (classFields.Contains(injectedClassName.Underscore()))
            {
                return;
            }

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(classPath));
            var rootNode =  syntaxTree.GetRoot();

            rootNode = AddUsing(rootNode, injectedClass);

            SyntaxTree modifiedSyntaxTree = CSharpSyntaxTree.Create((CSharpSyntaxNode)rootNode);

            var compilationUnit = (CompilationUnitSyntax)modifiedSyntaxTree.GetRoot();

            var namespaceDeclaration = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            var classDeclaration = namespaceDeclaration?.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

           


            var className = classDeclaration.Identifier.ValueText;

            FieldDeclarationSyntax fieldDeclaration = GetFieldDeclaration(injectedClassName);

            ConstructorDeclarationSyntax constructorDeclaration = GetConstructorDeclaration(className, injectedClassName);

            ConstructorDeclarationSyntax? constructor = classDeclaration?.DescendantNodes()
            .OfType<ConstructorDeclarationSyntax>()
            .FirstOrDefault();

            ClassDeclarationSyntax updatedClassDeclaration = null;

            if (constructor != null)
            {
                updatedClassDeclaration = classDeclaration.InsertNodesBefore(constructor, new[] { fieldDeclaration });
            }
            else
            {                

                var newConstructor = GetConstructorDeclaration(className, injectedClassName);

                MethodDeclarationSyntax? firstMethod = classDeclaration.DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .FirstOrDefault();

                if (firstMethod != null)
                {
                    updatedClassDeclaration = classDeclaration.InsertNodesBefore(firstMethod, new[] { newConstructor });

                    var constr = updatedClassDeclaration.DescendantNodes()
                        .OfType<ConstructorDeclarationSyntax>()
                        .FirstOrDefault();

                    updatedClassDeclaration = updatedClassDeclaration.InsertNodesBefore(constr, new[] { fieldDeclaration });
                }
                else
                {
                    updatedClassDeclaration = classDeclaration.AddMembers(fieldDeclaration);
                    updatedClassDeclaration = updatedClassDeclaration.AddMembers(newConstructor);
                }

            }

            var updatedRoot = rootNode.ReplaceNode(classDeclaration, updatedClassDeclaration).NormalizeWhitespace();

            File.WriteAllText(classPath, CodeGeneratorHandler.RemoveComment(updatedRoot.ToString()));
        }

        private static ConstructorDeclarationSyntax GetConstructorDeclaration(string className, string injectedClassName)
        {
            var body = SyntaxFactory.ParseStatement($"{injectedClassName.Underscore()}={injectedClassName.ToLowerFirst()};");
            var parameter = SyntaxFactory.Parameter(SyntaxFactory.Identifier(injectedClassName.ToLowerFirst()))
            .WithType(SyntaxFactory.ParseTypeName($"I{injectedClassName}"));

            return SyntaxFactory.ConstructorDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .WithBody(SyntaxFactory.Block(body));
        }

        private static FieldDeclarationSyntax GetFieldDeclaration(string className)
        {
            var identifier = SyntaxFactory.Identifier(className.Underscore());
            var type = SyntaxFactory.ParseTypeName($"I{className}");

            return SyntaxFactory.FieldDeclaration(
             SyntaxFactory.VariableDeclaration(type)
                 .WithVariables(SyntaxFactory.SingletonSeparatedList(
                     SyntaxFactory.VariableDeclarator(identifier)
                 ))
            ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));
        }

        private static SyntaxNode AddUsing(SyntaxNode rootNode, string injectedClass)
        {
            var usingStatements = rootNode.DescendantNodes().OfType<UsingDirectiveSyntax>().ToList();
            var usingStatementsList = usingStatements?.Select(x => x.Name.ToString()).ToList();

            CodeNamespace codeNamespace = new CodeNamespace();

            var injectedClassNameSpace = DirectoryHandler.GetClassNameSpace(injectedClass);

            if (!usingStatementsList.Contains(injectedClassNameSpace))
            {
                var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(injectedClassNameSpace));
                rootNode = ((CompilationUnitSyntax)rootNode).AddUsings(newUsing);
                return rootNode;
            }

            return rootNode;
        }
    }
}