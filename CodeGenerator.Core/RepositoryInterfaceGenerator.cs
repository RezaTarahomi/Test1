using System.CodeDom;

namespace CodeGenerator.Core
{
    public class RepositoryInterfaceGenerator
    {
        public static void Generate(string entityName, string path, string type)
        {
            var interfaceName = "I" + entityName + type;
            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            var entityUsing = ProjectStructure.Domain + "." + ProjectStructure.EntitiesFolder;
            CodeNamespace blankNamespaces = new CodeNamespace();
            blankNamespaces.Imports.Add(new CodeNamespaceImport(entityUsing));
            compileUnit.Namespaces.Add(blankNamespaces);

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(path.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);


            CodeTypeDeclaration newClass = new CodeTypeDeclaration(interfaceName);
            newClass.IsInterface = true;

            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, interfaceName, path);

        }
    }
}
