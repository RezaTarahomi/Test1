using CodeGenerator.Core;
using CodeGenerator.Core.Extensions;
using CodeGenerator.Test.Unit.TestBuilders;
using Database.Data.Entities;
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
        private string domainName;
        private string entityName;
        public ApiGeneratorTests()
        {
            domainName = "VehicleDomain";
            entityName = "Vehicles_Test";
        }
        #region Create Request Tests       

        [Fact]
        public void create_lis_query_request()
        { 
            var apiType = ApiType.GetList;

            var responseFields = new EntityBuilder()
                .WithName("List<Id_Caption>")
               .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, responseModel:responseFields);

            var expected = File.Exists(GetRequestFullPath(apiType));            

            Assert.True(expected);           

        }               

        [Fact]
        public void create_get_getById_query_request()
        {
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

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields, responseFields);            

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);
        }

        private string? GetRequestFullPath(ApiType apiType)
        {
            var projectName = ProjectStructure.Application;

            var requestType= ApiGenerator.GetRequestType(apiType);
            var requestName= ApiGenerator.GetRequestName(apiType,entityName,null);

            var createQueryRequest = requestName + requestType;

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(),
                projectName, "Features",
                domainName,
                entityName.GetPluralForm(), requestType.GetPluralForm(),
                requestName,
                createQueryRequest + ".cs");

            return fullPath;
        }
        private string? GetHandlerFullPath(ApiType apiType)
        {
            var projectName = ProjectStructure.Application;

            var requestType = ApiGenerator.GetRequestType(apiType);
            var requestName = ApiGenerator.GetRequestName(apiType, entityName, null);

            var createQueryRequest = requestName + requestType;

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(),
                projectName, "Features",
                domainName,
                entityName.GetPluralForm(), requestType.GetPluralForm(),
                requestName,
                createQueryRequest + "Handler.cs");

            return fullPath;
        }

        [Fact]
        public void create_get_for_grid_query_request()
        {
            
            var apiType = ApiType.GetForGrid;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Filter", "FilterModel")
                .Build();

            var responseFields = (new EntityBuilder())
                .WithProperty("Id", typeof(int))
                .WithProperty("Name", typeof(string))
                .WithProperty("Capacity", typeof(int))
                .WithProperty("VehicleModel_TestId", typeof(int))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields, responseFields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);          

        }

        [Fact]
        public void create_create_request()
        {
            var apiType = ApiType.Create;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Id", typeof(int))
                .WithProperty("Name", typeof(string))
                .WithProperty("Capacity", typeof(int))
                .WithProperty("VehicleModel_TestId", typeof(int))
                .Build();            

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);           

        }

        [Fact]
        public void create_edit_request()
        {

            var apiType = ApiType.Edit;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Id", typeof(int))
                .WithProperty("Name", typeof(string))
                .WithProperty("Capacity", typeof(int))
                .WithProperty("VehicleModel_TestId", typeof(int))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);           
        }

        [Fact]
        public void create_delete_by_Id_request()
        {

            var apiType = ApiType.DeleteById;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Id", typeof(int))                
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);            

        }

        [Fact]
        public void create_delete_by_Ids_request()
        {

            var apiType = ApiType.DeleteByIds;

            var requestfields = (new EntityBuilder())
                .WithName(null)
                .WithProperty("Ids", typeof(List<int>))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);            

        }


        [Fact]
        public void create_active_request()
        {
            var apiType = ApiType.Active;

            var requestfields = (new EntityBuilder())
                .WithName("Active"+entityName)
                .WithProperty("Id", typeof(int))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);            

        }

        [Fact]
        public void create_deactive_request()
        { 

            var apiType = ApiType.Deactive;

            var requestfields = (new EntityBuilder())
                .WithName("Deactive" + entityName)
                .WithProperty("Id", typeof(int))
                .Build();

            ApiGenerator.GenerateRequest(domainName, entityName, apiType, requestfields);

            var expected = File.Exists(GetRequestFullPath(apiType));

            Assert.True(expected);           

        }
        #endregion

        #region Create Handler Tests

        [Fact]
        public void create_lis_query_handler()
        {
            var apiType = ApiType.GetList;
            var expectedHandlerName = "Get" + entityName.GetPluralForm() + "ListQueryHandler";
            var expectedHandlerBase = $"IRequestHandler<{"Get" + entityName.GetPluralForm() + "ListQuery"},List<Id_Caption>>";

            var responseFields = new EntityBuilder()
                .WithName("List<Id_Caption>")
               .Build();

            ApiGenerator.GenerateHandler(domainName, entityName, apiType,responseModel:responseFields);

            var expected = File.Exists(GetHandlerFullPath(apiType));

            Assert.True(expected);
            Assert.Equal(expectedHandlerName, CodeGeneratorHandler.GetClassName(GetHandlerFullPath(apiType)));
            Assert.Equal(expectedHandlerBase, CodeGeneratorHandler.GetClassBaseTypeName(GetHandlerFullPath(apiType)));
        }

        [Fact]
        public void create_command_handler()
        {
            var apiType = ApiType.Create;
            var expectedHandlerName = "Create" + entityName + "CommandHandler";
            var expectedHandlerBase = $"IRequestHandler<{"Create" + entityName + "Command"}>";
            

            ApiGenerator.GenerateHandler(domainName, entityName, apiType);

            var expected = File.Exists(GetHandlerFullPath(apiType));

            Assert.True(expected);
            Assert.Equal(expectedHandlerName,CodeGeneratorHandler.GetClassName(GetHandlerFullPath(apiType)));
            Assert.Equal(expectedHandlerBase, CodeGeneratorHandler.GetClassBaseTypeName(GetHandlerFullPath(apiType)));
        } 

        

        #endregion
    }
}
