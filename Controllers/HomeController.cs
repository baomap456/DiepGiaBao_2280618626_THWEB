using Microsoft.AspNetCore.Mvc;

namespace THLapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}