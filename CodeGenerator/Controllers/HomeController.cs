using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
