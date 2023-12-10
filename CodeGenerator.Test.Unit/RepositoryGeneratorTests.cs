using CodeGenerator.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeGenerator.Test.Unit
{
    public class RepositoryGeneratorTests
    {
        [Fact]
        public void generate_repository_class()
        {

            var repositoryProjectName = ProjectStructure.Persistance;

            var repositoryInterfaceProjectName = ProjectStructure.Application;
        

            var domainName = "VehicleDomain";

            var repositoryPath = repositoryProjectName + "\\" + domainName;
            var repositoryInterfacePath = repositoryInterfaceProjectName + "\\" + domainName;

            var entityName = "Vehicles_Test";

            RepositoryInterfaceGenerator.Generate(entityName, repositoryInterfacePath);

            RepositoryGenerator.Generate(entityName, repositoryPath);

            var className = entityName + "Repository";

            var fullRepositoryPath = Path.Combine(DirectoryHandler.GetAppRoot(), repositoryPath, className + ".cs");

            var expected = File.Exists(fullRepositoryPath);

            Assert.True(expected);

            if (File.Exists(fullRepositoryPath))
            {
                File.Delete(fullRepositoryPath);
            }

            var interfaceName = "I" + entityName + "Repository";
            var fullRepositoryInterfacePath = Path.Combine(DirectoryHandler.GetAppRoot(), repositoryInterfacePath, interfaceName + ".cs");

            if (File.Exists(fullRepositoryInterfacePath))
            {
                File.Delete(fullRepositoryInterfacePath);
            }
        }

    }
}
