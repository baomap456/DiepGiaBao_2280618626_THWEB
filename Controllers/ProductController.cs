using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using THLapTrinhWeb.Models;
using THLapTrinhWeb.Repositories;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }


    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    // [HttpPost]
    // public IActionResult Add(Product product)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         _productRepository.Add(product);
    //         return RedirectToAction("Index"); /
    //     }
    //     return View(product);
    // }

    // Các actions khác như Display, Update, Delete 

    // Display a list of products 
    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }

    // Display a single product 
    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Show the product update form 
    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Process the product update 
    [HttpPost]
    public async Task<IActionResult> Update(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        // if (ModelState.IsValid)
        // {
        //     _productRepository.Update(product);
        //     return RedirectToAction("Index");
        // }
        // return View(product);
        if (ModelState.IsValid)
        {
            if (imageUrl!= null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            if (imageUrls!= null)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));
                }
            }
            _productRepository.Update(product);
           return RedirectToAction("Index");
        }
        return View(product);

    }

    // Show the product delete confirmation 
    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    // Process the product deletion 
    [HttpPost, ActionName("DeleteConfirmed")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        if (ModelState.IsValid)
        {
            if (imageUrl!= null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            if (imageUrls!= null)
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));
                }
            }
            _productRepository.Add(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }
    private async Task<string> SaveImage(IFormFile image)
    {

        var savePath = Path.Combine("wwwroot/images", image.FileName);
        using (var fileStream = new FileStream(savePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return "/images/" + image.FileName;
    }
}