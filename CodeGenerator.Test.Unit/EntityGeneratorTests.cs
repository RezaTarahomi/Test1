using Application.Entities;
using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Core.Extensions;
using CodeGenerator.Test.Unit.TestBuilders;
using Database.Data.Entities;
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
            Assert.Equal("int", sut.First().Type);

            Assert.Equal("Name", sut.Last().Name);
            Assert.Equal("string", sut.Last().Type);
        }

        [Fact]
        public void generate_entity()
        {
            var path = "Application\\Entities";

            var entityName = "Vehicles_Test";
            var name = "Name";
            var nameType = "string";

            var capacity = "Capacity";
            var capacityType = "int";



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
            Assert.Equal("int", sut.First().Type);
            
        }


        [Fact]
        public void inject_class()
        {

            var classPath = @"E:\DotNetProjects\Test\CodeGenerator\Application\Features\VehicleDomain\Vehicles_Tests\Commands\CreateVehicles_Test\CreateVehicles_TestCommandHandler.cs";
            var injectedClass = @"E:\DotNetProjects\Test\CodeGenerator\Application\VehicleDomain\IVehicles_TestQuery.cs";
           
            EntityGenerator.Inject(classPath, injectedClass);

            var injectedclassName = Path.GetFileNameWithoutExtension(injectedClass).Substring(1).Underscore();
            var classFields = CodeGeneratorHandler.GetClassProperties(classPath).Select(x=>x.Name).ToList();
            Assert.Contains(injectedclassName,classFields);

        }
        [Fact]
        public void call_repository_method_in_class_method()
        {
            var baseClassPath = @"E:\DotNetProjects\Test\CodeGenerator\Application\Features\VehicleDomain\Vehicles_Tests\Commands\CreateVehicles_Test\CreateVehicles_TestCommandHandler.cs";
            var injectedClassInterface = @"E:\DotNetProjects\Test\CodeGenerator\Application\VehicleDomain\IVehicles_TestQuery.cs";
            var injectedClass = @"E:\DotNetProjects\Test\CodeGenerator\Persistance\VehicleDomain\Vehicles_TestQuery.cs";

            var injectedClassMethod = new MethodSignBuilder()
                .WithName("GetAll")
                .WithResponseType("List<Vehicles_Test>")
                .WithResponseVariableName("vehicles_Tests")                
                .Build();

            var baseClassMethodName = "Handle";

            EntityGenerator.CallInjectedClassMethod(baseClassPath, baseClassMethodName, injectedClass, injectedClassInterface, injectedClassMethod);

            Assert.Contains(injectedClassMethod.Name, File.ReadAllText(baseClassPath));
         

        }

        [Fact]
        public void find_entity_parent()
        {
            var entityDirectorypath = @"E:\DotNetProjects\Test\CodeGenerator\Database\Data\Entities\";

            var expected = new List<Entity> 
            {
                new Entity 
                {
                    Name = "ResponseParameter" ,
                    Fields=new List<Field>
                    {
                        new Field {Name ="Id", Type = "int"}
                    },
                    EntityParents=new List<EntityParent>
                    {
                        new EntityParent
                        {
                            Entity=new Entity{Name="Api"} , OneToOne=false
                        }
                    }
                }              

            };

            List<Entity> sut = EntityGenerator.GetEntitiesFromDirectory(entityDirectorypath);

            var result = sut.FirstOrDefault(x => x.Name == "ResponseParameter").EntityParents.FirstOrDefault(x=>x.Parent.Name=="Api").Parent.Name ;

            Assert.Equal("Api", result);
        }
        [Fact]
        public void find_enum()
        {
            var entityDirectorypath = @"E:\DotNetProjects\Test\CodeGenerator\Database\Data\Entities\";

            var expected = new List<Entity>
            {
                new Entity
                {
                    Name = "Api" ,
                    Fields=new List<Field>
                    {
                        new Field {Name ="Type", Type = "ApiType"}
                    }
                }

            };

            List<Entity> sut = EntityGenerator.GetEntitiesFromDirectory(entityDirectorypath);

            var result = sut.FirstOrDefault(x => x.Name == "Api").Fields.FirstOrDefault(x => x.Name == "Type").EnumType.EnumFields.FirstOrDefault(x=>x.Name== "GetForGrid").Value;

            Assert.Equal(1, result.Value);
        }

        [Fact]
        public void generate_enum()
        {
            var path = "Database\\Entities";

            var enumName = "AppType";

            var enumType = new EnumType
            {
                Name = enumName,
               
                EnumFields = new List<EnumField>
                {
                    new EnumField{ Name="Kiosk", Value=1},
                    new EnumField{ Name="Panel", Value=2},
                }
            };

            EntityGenerator.GenerateEnum(path, enumType);

            var fullPath = Path.Combine(DirectoryHandler.GetAppRoot(), path, enumName + ".cs");

            var expected = File.Exists(fullPath);

            Assert.True(expected);

            //if (File.Exists(fullPath))
            //{
            //    File.Delete(fullPath);
            //}

        }

        //[Fact]
        //public void addParent
    }
}
