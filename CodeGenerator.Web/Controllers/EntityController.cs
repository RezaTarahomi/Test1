using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Web.Controllers
{
    public class EntityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
