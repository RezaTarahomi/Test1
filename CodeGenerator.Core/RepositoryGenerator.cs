using CodeGenerator.Core.Extensions;
using System.CodeDom;

namespace CodeGenerator.Core
{
    public class RepositoryGenerator
    {
        public static void Generate(string entityName, string path,string type)
        {
            var className = entityName + type;
            var interfaceName = "I" + entityName + type;
            // Create a CodeCompileUnit
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace blankNamespaces = new CodeNamespace();

            var entityUsing = ProjectStructure.Domain + "." + ProjectStructure.EntitiesFolder;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(entityUsing));

            var domainName = "VehicleDomain";

            var interfaceUsing = ProjectStructure.Application + "." + domainName;
            blankNamespaces.Imports.Add(new CodeNamespaceImport(interfaceUsing));

            blankNamespaces.Imports.Add(new CodeNamespaceImport("System.Linq"));
            blankNamespaces.Imports.Add(new CodeNamespaceImport("Microsoft.EntityFrameworkCore"));

            compileUnit.Namespaces.Add(blankNamespaces);

            // Create a namespace
            CodeNamespace codeNamespace = new CodeNamespace(path.Replace("\\", "."));
            compileUnit.Namespaces.Add(codeNamespace);

            CodeTypeDeclaration newClass = new CodeTypeDeclaration(className);
            newClass.IsClass = true;

            newClass.BaseTypes.Add(new CodeTypeReference(interfaceName));

            CodeMemberField queriableField = new CodeMemberField($"IQueryable<{entityName}>", entityName.Underscore()+"s");
            queriableField.Attributes = MemberAttributes.Private;           

            newClass.Members.Add(queriableField);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression( "TransportDbContext", "dbcontext"));
            constructor.Statements.Add(new CodeSnippetExpression($"{entityName.Underscore()}s = dbcontext.Set<{entityName}>()"));
            newClass.Members.Add(constructor);


            codeNamespace.Types.Add(newClass);

            CodeGeneratorHandler.GenerateCSharpClassFile(compileUnit, className, path);
        }
    }
}
