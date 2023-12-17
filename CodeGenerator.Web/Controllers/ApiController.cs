using CodeGenerator.Core;
using CodeGenerator.Core.Dtos;
using CodeGenerator.Web.Models;
using Database.Data;
using Database.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


            return View(apies);
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

        [HttpPost]
        public async Task<IActionResult> Create(ApiViewModel model)
        {

            try
            {


            }
            catch (Exception ex)
            {

                throw;
            }




            string directoryPath = @"E:\DotNetProjects\Test\CodeGenerator\CodeGenerator\Controllers";
            model.Controllers = Directory.GetFiles(directoryPath).Select(x =>
            new FileModel
            {
                Name = Path.GetFileName(x),
                Path = x
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> CreateApi()
        {           

            return View(new CreateApiViewModel() );
        }

        [HttpPost]
        public async Task<IActionResult> CreateApi(CreateApiViewModel model)
        {

            try
            {
                var isExistApi = await _context.Apis.AnyAsync(x=>x.Domain==model.Domains && x.Name==model.Name);
                if (isExistApi)
                    return View (model);

              await  _context.Apis.AddAsync(new Database.Data.Entities.Api
                {
                    Domain = model.Domains,
                    Name = model.Name,
                    Url = model.Url,
                    Type = model.ApiType

                });
               await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }




          

            return View(model);
        }
        public async Task<IActionResult> RequestParameter(int apiId)
        {

            return View(new RequestParameterViewModel()
            {
                ApiId = apiId
            });
        }

        [HttpPost]
        public async Task<IActionResult> RequestParameter(RequestParameterViewModel model)
        {

            try
            {
                var isExistApi = await _context.RequestParameters.AnyAsync(x => x.ApiId == model.ApiId && x.Name == model.Name);
                if (isExistApi)
                    return View(model);

                await _context.RequestParameters.AddAsync(new Database.Data.Entities.RequestParameter
                {
                   
                    Name = model.Name,                    
                    Type = model.Type,
                    ApiId = model.ApiId,
                    Filterable = model.Filterable,
                    Sortable = model.Sortable,
                    IsRequired = model.IsRequired,
                    Description = model.Description,
                    AttributeType = model.AttributeType,

                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }






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

        public async Task<IActionResult> GetBaseTypeFields(BaseType baseType)
        {
           
            string path=null;

            if (baseType== BaseType.BaseEntity)
            {
                 path = @"E:\DotNetProjects\Test\CodeGenerator\Application\Common\BaseEntity.cs";
            }
            else if(baseType== BaseType.BaseAudit)
            {
                path = @"E:\DotNetProjects\Test\CodeGenerator\Application\Common\BaseAudit.cs";
            }

            var fields= CodeGeneratorHandler.GetClassProperties(path).Select(x => new FieldViewModel
            {
                Name = x.Name,
                Type = x.Type,
            }).ToList();

            return PartialView("_Fields",fields);
        }

        public async Task<IActionResult> CallMethod()
        {
            ViewBag.Methods = new List<ValueText>();
            ViewBag.RepositoryMethods = new List<ValueText>();
            var model = new CallMethodViewModel() { Path= @"E:\DotNetProjects\Test\CodeGenerator\Application\Features\VehicleDomain\Vehicles_Tests\Commands\CreateVehicles_Test\CreateVehicles_TestCommandHandler.cs" };

            ViewBag.Repositories = Directory.GetFiles(@"E:\DotNetProjects\Test\CodeGenerator\Persistance\VehicleDomain").Select(x =>
               new FileModel
               {
                   Name = Path.GetFileName(x),
                   Path = x
               }).ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CallMethod(CallMethodViewModel model)
        {
            ViewBag.Methods = new List<ValueText>();
            ViewBag.RepositoryMethods = new List<ValueText>();

            ViewBag.Repositories = Directory.GetFiles(@"E:\DotNetProjects\Test\CodeGenerator\Persistance\VehicleDomain").Select(x =>
               new FileModel
               {
                   Name = Path.GetFileName(x),
                   Path = x
               }).ToList();



            return View(model);
        }




        private object List<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class CallMethodViewModel
    {
        public string Path { get; set; }
        public string Method { get; set; }
        public string Repository { get; set; }
        public string RepositoryMethod { get; set; }
        public string ResponseType { get; set; }
        public string Parameters { get; set; }


    }

    public class ValueText
    {
        public string value { get; set; }
        public string text { get; set; }

    }


}
