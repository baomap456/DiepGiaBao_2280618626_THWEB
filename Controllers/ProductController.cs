using THLapTrinhWeb.Models;
// using THLapTrinhWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace THLapTrinhWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository,ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl,List<IFormFile> Images,string MediaUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage 
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await _productRepository.AddAsync(product);
                
                if (!string.IsNullOrEmpty(MediaUrl))
                {
                    var isVideo = MediaUrl.EndsWith(".mp4") || MediaUrl.EndsWith(".webm") || MediaUrl.EndsWith(".ogg")
                                                            || MediaUrl.Contains("youtube.com")
                                                            || MediaUrl.Contains("youtu.be");;
                    var productImage = new ProductImage
                    {
                        Url = MediaUrl,
                        ProductId = product.Id,
                        IsVideo = isVideo
                    };
                    await _context.ProductImages.AddAsync(productImage);
                    await _context.SaveChangesAsync();
                }

                if (Images != null && Images.Count > 0)
                {
                    foreach (var file in Images)
                    {
                        var imagePath = await SaveImage(file);
                        var productImage = new ProductImage
                        {
                            Url = imagePath,
                            ProductId = product.Id
                        };
                        await _context.ProductImages.AddAsync(productImage);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập 
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            // Thay đổi đường dẫn theo cấu hình của bạn 
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl,List<int> ExistingImageIds,List<bool> IsVideos,List<IFormFile> UpdatedImageFiles)
        {

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingProduct = await _productRepository.GetByIdAsync(id);

                // Cập nhật các thông tin khác của sản phẩm 
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                for (int i = 0; i < ExistingImageIds.Count; i++)
                {
                    var image = existingProduct.Images.FirstOrDefault(img => img.Id == ExistingImageIds[i]);
                    if (image != null)
                    {
                        if (UpdatedImageFiles != null && i < UpdatedImageFiles.Count && UpdatedImageFiles[i] != null)
                        {
                            image.Url = await SaveImage(UpdatedImageFiles[i]);
                            image.IsVideo = false;
                        }
                    }
                }

                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm 
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}