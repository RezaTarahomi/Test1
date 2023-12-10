using CodeGenerator.Core;
using CodeGenerator.Test.Unit.TestBuilders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeGenerator.Test.Unit
{
    public class RepositoryInterfaceGeneratorTests
    {
        [Fact]
        public void create_interface()
        {
            var projectName = ProjectStructure.Application;
            var domainName = "VehicleDomain";

            var path = projectName + "\\" + domainName;

            var entityName = "Vehicles_Test";            

           RepositoryInterfaceGenerator.Generate(entityName, path);


            var interfaceName = "I" + entityName + "Repository";

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(), path, interfaceName + ".cs");

            var expected = File.Exists(fullPath);

            Assert.True(expected);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

        }
    }
}
