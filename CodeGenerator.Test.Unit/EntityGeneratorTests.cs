using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Test.Unit.TestBuilders;
using Database.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CodeGenerator.Test.Unit
{
    public class EntityGeneratorTests
    {
        [Fact]
        public void property_list()
        {
            var path = "Application\\Entities";

            var entity = (new EntityBuilder())
                .WithName("Vehicles_Test")
                .WithProperty("Name", typeof(string))
                .WithProperty("Capacity", typeof(int))
                .Build();

            EntityGenerator.GenerateEntity(path, entity);

            var filePath = Path.Combine(path, entity.Name + ".cs");

            var sut= CodeGeneratorHandler.GetClassProperties(filePath).OrderBy(x=>x.Name).ToList();

            Assert.Equal(2, sut.Count);
            Assert.Equal("Capacity", sut.First().Name);
            Assert.Equal(typeof(int), sut.First().Type);

            Assert.Equal("Name", sut.Last().Name);
            Assert.Equal(typeof(string), sut.Last().Type);
        }

        [Fact]
        public void generate_entity()
        {
            var path = "Application\\Entities";

            var entityName = "Vehicles_Test";
            var name = "Name";
            var nameType = typeof(string);

            var capacity = "Capacity";
            var capacityType = typeof(int);



            var entityCreateModel = new EntityCreateModel
            {
                Name = entityName,
                Properties = new List<ClassPropertyModel>
                {
                    new ClassPropertyModel{ Name=name, Type=nameType},
                    new ClassPropertyModel{ Name=capacity, Type=capacityType},
                }
            };

            EntityGenerator.GenerateEntity(path, entityCreateModel);

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(), path, entityName + ".cs");            

            var expected = File.Exists(fullPath);

            Assert.True(expected);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

        }

        [Fact]
        public void generate_nameSpace_from_fullPath()
        {
            var path = "E:/DotNetProjects/Test/CodeGenerator/Application/Entities";
            var expected = "Application.Entities";

            var sut=DirectoryHandler.GetNameSpace(path);

            Assert.Equal(expected, sut);

        }

        [Fact]
        public void property_list_with_relation()
        {
            var path = "Application\\Entities";

            var model = (new EntityBuilder())
                .WithName("VehicleModel_Test")
                .WithProperty("Id", typeof(int))
                .WithProperty("Name", typeof(string))
                .WithProperty("Caption", typeof(string))
                .Build();

            var vehicle = (new EntityBuilder())
                .WithName("Vehicles_Test")
                .WithProperty("Id", typeof(int)) 
                .WithProperty("Name", typeof(string)) 
                .WithProperty("Capacity", typeof(int)) 
                .WithProperty("VehicleModel_TestId", typeof(int)) 
                .WithProperty("VehicleModel_Test", typeof(VehicleModel_Test)) 
                .Build(); 

            EntityGenerator.GenerateEntity(path, model);
            EntityGenerator.GenerateEntity(path, vehicle);

            var filePath = Path.Combine(path, vehicle.Name + ".cs");

            var sut = CodeGeneratorHandler.GetClassProperties(filePath).OrderBy(x => x.Name).ToList();

            Assert.Equal(2, sut.Count);
            Assert.Equal("Capacity", sut.First().Name);
            Assert.Equal(typeof(int), sut.First().Type);
            
        }
    }
}
