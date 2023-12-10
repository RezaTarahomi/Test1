using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace CodeGenerator.Test.Unit
{
    public class DirectoryHandlerTest
    {
        [Fact]
        public void create_directory()
        {
            var path = "E:/DotNetProjects/Test/CodeGenerator/Application";
            var fileName = "Vehicles_Test";
            var newPath = Path.Combine(path, fileName);          

            DirectoryHandler.CreateDirectory(path, fileName);

            Assert.True(Directory.Exists(newPath));

            if (Directory.Exists(newPath))
            {
                Directory.Delete(newPath);
            }

        }

        [Fact]
        public void get_views_od_controller()
        {
            string directoryPath = @"E:\DotNetProjects\Test\CodeGenerator\CodeGenerator\Controllers";
            var controllerPath = Directory.GetFiles(directoryPath).FirstOrDefault();
         

            List<FileModel> sut = DirectoryHandler.GetViewFiles(controllerPath);  
        }
    }
}