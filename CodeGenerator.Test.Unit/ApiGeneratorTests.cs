using CodeGenerator.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CodeGenerator.Test.Unit
{
    public class ApiGeneratorTests
    {
        [Fact]
        public void create_get_Api()
        {
            var entityDomain = "VehicleDomain";
            var entityName = "Vehicle_Test";
            var controllerName = entityName + "Controller";

            var apiPath = ProjectStructure.ApiPath;
            var controllerPath = Path.Combine(DirectoryHandler.GetAppRoot(), apiPath , controllerName + ".cs");
                      

            var apiModels = new List<CreateApiMethodModel>
            {
                new CreateApiMethodModel
                {                    
                    Type = ApiType.GetList,
                    EntityName = "Vehicle_Test2",
                }
            };

            ApiGenerator.Generate(controllerPath, apiModels);
           // ApiGenerator.GenerateDocument(controllerPath, entityName, apiType);

            var expectedApi= CodeGeneratorHandler.GetClassMethodNames(controllerPath);

            expectedApi.Should().Contain("Get"+ entityName+"s");
            
        }
    }
}
