using CodeGenerator.Core;
using CodeGenerator.Test.Unit.TestBuilders;
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
            var controllerPath = Path.Combine(DirectoryHandler.GetAppRoot(), apiPath, controllerName + ".cs");


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

            var expectedApi = CodeGeneratorHandler.GetClassMethodNames(controllerPath);

            expectedApi.Should().Contain("Get" + entityName + "s");

        }

        [Fact]
        public void create_lis_query_request()
        {
            //GetVehiclesQuery
            //GeVehiclesForGridQuery
            
            var domainName = "VehicleDomain";
            
            var entityName = "Vehicles_Test";

            var apiType = ApiType.GetList;

            
            ApiGenerator.GenerateRequest(domainName, entityName, apiType);

            var projectName = ProjectStructure.Application;
            var createQueryRequest = "Get" + DirectoryHandler.GetPluralForm(entityName)+"List" + "Query";

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(),
                projectName,"Features", 
                domainName, 
                DirectoryHandler.GetPluralForm(entityName),"Queries",
                "Get" + DirectoryHandler.GetPluralForm(entityName),
                createQueryRequest + ".cs");

            var expected = File.Exists(fullPath);

            Assert.True(expected);

            //if (File.Exists(fullPath))
            //{
            //    File.Delete(fullPath);
            //}

        }

        [Fact]
        public void create_getById_query_request()
        {
            //GetVehiclesQuery
            //GeVehiclesForGridQuery

            var domainName = "VehicleDomain";

            var entityName = "Vehicles_Test";

            var apiType = ApiType.GetById;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Id", typeof(int))
                .Build();

            var responseFields = (new EntityBuilder())                
                .WithProperty("Id", typeof(int))
                .WithProperty("Name", typeof(string))
                .WithProperty("Capacity", typeof(int))
                .WithProperty("VehicleModel_TestId", typeof(int))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType,requestfields,responseFields);

            var projectName = ProjectStructure.Application;
            var createQueryRequest = "Get" + entityName + "ById" + "Query";

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(),
                projectName, "Features",
                domainName,
                DirectoryHandler.GetPluralForm(entityName), "Queries",
                "Get" + entityName + "ById",
                createQueryRequest + ".cs");

            var expected = File.Exists(fullPath);

            Assert.True(expected);

            //if (File.Exists(fullPath))
            //{
            //    File.Delete(fullPath);
            //}

        }
    }
}
