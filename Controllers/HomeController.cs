using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using THLapTrinhWeb.Models;
using THLapTrinhWeb.Repositories;

namespace THLapTrinhWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 8 sản phẩm featured để hiển thị
            var featuredProducts = await _productRepository.GetAllAsync();

            // Lấy categories để hiển thị trong Shop by Category section
            var categories = await _categoryRepository.GetAllAsync();

            // Truyền categories qua ViewBag
            ViewBag.Categories = categories.Take(4).ToList(); // Chỉ lấy 4 categories đầu tiên

            return View(featuredProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}