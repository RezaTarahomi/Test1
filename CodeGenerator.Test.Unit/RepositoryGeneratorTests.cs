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
            var type = "Query";

            var repositoryPath = repositoryProjectName + "\\" + domainName;
            var repositoryInterfacePath = repositoryInterfaceProjectName + "\\" + domainName;

            var entityName = "Vehicles_Test";

            RepositoryInterfaceGenerator.Generate(entityName, repositoryInterfacePath,type);

            RepositoryGenerator.Generate(entityName, repositoryPath, type);

            var className = entityName + type;

            var fullRepositoryPath = Path.Combine(DirectoryHandler.GetAppRoot(), repositoryPath, className + ".cs");

            var expected = File.Exists(fullRepositoryPath);

            Assert.True(expected);

            //if (File.Exists(fullRepositoryPath))
            //{
            //    File.Delete(fullRepositoryPath);
            //}

            var interfaceName = "I" + entityName + type;
            var fullRepositoryInterfacePath = Path.Combine(DirectoryHandler.GetAppRoot(), repositoryInterfacePath, interfaceName + ".cs");

            //if (File.Exists(fullRepositoryInterfacePath))
            //{
            //    File.Delete(fullRepositoryInterfacePath);
            //}
        }

    }
}
