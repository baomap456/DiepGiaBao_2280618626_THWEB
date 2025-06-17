using THLapTrinhWeb.Models;
// using THLapTrinhWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace THLapTrinhWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            // Count products for each category
            var productCounts = new Dictionary<int, int>();
            foreach (var category in categories)
            {
                productCounts[category.Id] = products.Count(p => p.CategoryId == category.Id);
            }

            ViewBag.ProductCounts = productCounts;
            return View(categories);
        }

        public async Task<IActionResult> Display(int id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        public async Task<IActionResult> Delete(int id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();

            }
            return View(category);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
