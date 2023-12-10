using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Web.Models;
using Database.Data;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly CodeGeneratorDbContext _context;
        public ApiController(CodeGeneratorDbContext dbContext)
        {
            _context = dbContext;
        }
        public IActionResult Index()
        {
           
          

            var apies = _context.Apis.ToList();


            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = new ApiViewModel();

            string directoryPath = @"E:\DotNetProjects\Test\CodeGenerator\CodeGenerator\Controllers";
            model.Controllers = Directory.GetFiles(directoryPath).Select(x=>
            new FileModel
            {
                Name= Path.GetFileName(x),
                Path=x
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> GetViews(string path)
        {
           var views = DirectoryHandler.GetViewFiles(path);

            return Json(views);
        }

        public async Task<IActionResult> GetControllerMethods(string path)
        {
            var methods = CodeGeneratorHandler.GetClassMethodNames(path).Select(x => new {value=x,text=x }).ToList();

            return Json(methods);
        }
    }
}
