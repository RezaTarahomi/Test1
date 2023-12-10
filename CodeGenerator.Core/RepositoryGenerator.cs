using System.CodeDom;

namespace CodeGenerator.Core
{
    public class RepositoryGenerator
    {
        public static void Generate(string entityName, string path)
        {
            var className = entityName + "Repository";
            var interfaceName = "I" + entityName + "Repository";
            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace blankNamespaces = new CodeNamespace();

            var entityUsing = ProjectStructure.Domain + "." + ProjectStructure.EntitiesFolder;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(entityUsing));

            var domainName = "VehicleDomain";

            var interfaceUsing = ProjectStructure.Application + "." + domainName;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(interfaceUsing));

            blankNamespaces.Imports.Add(new CodeNamespaceImport("System.Linq"));

            compileUnit.Namespaces.Add(blankNamespaces);

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(path.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);

            CodeTypeDeclaration newClass = new CodeTypeDeclaration(className);
            newClass.IsClass = true;

            newClass.BaseTypes.Add(new CodeTypeReference(interfaceName));

            CodeMemberField queriableField = new CodeMemberField($"IQueryable<{entityName}>", entityName.ToLower()+"s");
            queriableField.Attributes = MemberAttributes.Private;           

            newClass.Members.Add(queriableField);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression( "TransportDbContext", "dbcontext"));
            constructor.Statements.Add(new CodeSnippetExpression($"{entityName.ToLower()}s = dbcontext.Set<{entityName}>()"));
            newClass.Members.Add(constructor);


            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, className, path);
        }
    }
}
