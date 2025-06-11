using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using THLapTrinhWeb.Extensions;
using THLapTrinhWeb.Models;

namespace THLapTrinhWeb.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var product = await GetProductFromDatabase(productId);

                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction("Index");
                }

                var cartItem = new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl // Thêm ImageUrl
                };

                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
                cart.AddItem(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                TempData["SuccessMessage"] = $"{product.Name} added to cart successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the product to cart.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

                if (cart is not null)
                {
                    cart.RemoveItem(productId);
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                    TempData["SuccessMessage"] = "Item removed from cart successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cart is empty.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the item.";
            }

            return RedirectToAction("Index");
        }

        // Thêm method UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

                if (cart != null)
                {
                    cart.UpdateQuantity(productId, quantity);
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                    TempData["SuccessMessage"] = "Cart updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cart is empty.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the cart.";
            }

            return RedirectToAction("Index");
        }

        // Thêm method ClearCart
        public IActionResult ClearCart()
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

                if (cart != null && cart.Items.Any())
                {
                    cart.Clear();
                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                    TempData["SuccessMessage"] = "Cart cleared successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cart is already empty.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while clearing the cart.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {

            // Bypass ModelState validation for now (vì có UserId và OrderDetails validation issues)
            if (string.IsNullOrWhiteSpace(order.ShippingAddress))
            {
                TempData["ErrorMessage"] = "Please enter shipping address.";
                return View(order);
            }

            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

                if (cart == null || !cart.Items.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Index");
                }

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    TempData["ErrorMessage"] = "Please login to place order.";
                    return RedirectToAction("Index");
                }

                // Create order với OrderDetails
                var newOrder = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = cart.GetTotalPrice(),
                    ShippingAddress = order.ShippingAddress.Trim(),
                    Notes = order.Notes?.Trim() ?? ""
                };

                // Tạo OrderDetails từ cart items
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Order = newOrder  // Set relationship
                    };
                    newOrder.OrderDetails.Add(orderDetail);

                }

                // Add order (EF sẽ tự động add OrderDetails)
                _context.Orders.Add(newOrder);
                var result = await _context.SaveChangesAsync();
                // Log OrderDetails IDs sau khi save
                if (newOrder.Id > 0)
                {
                    // Clear cart
                    HttpContext.Session.Remove("Cart");

                    TempData["SuccessMessage"] = $"Order #{newOrder.Id} with {newOrder.OrderDetails.Count} items placed successfully!";

                    return View("OrderCompleted", newOrder.Id);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save order.";
                    return View(order);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(order);
            }
        }

        // Thêm method OrderCompleted
        public async Task<IActionResult> OrderCompleted(int orderId)
        {
            Console.WriteLine($"OrderCompleted called with orderId: {orderId}");

            if (orderId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid order ID.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Lấy đầy đủ thông tin order với OrderDetails và Product
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.ApplicationUser)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Check if user owns this order
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser?.Id != order.UserId)
                {
                    TempData["ErrorMessage"] = "Access denied.";
                    return RedirectToAction("Index", "Home");
                }

                return View(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OrderCompleted: {ex.Message}");
                TempData["ErrorMessage"] = "Error loading order details.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Test method
        public IActionResult TestOrderCompleted()
        {
            return RedirectToAction("OrderCompleted", new { orderId = 123 });
        }

        [HttpPost]
        public async Task<IActionResult> TestCheckout()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Content("User not found");
                }

                var testOrder = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = 100000,
                    ShippingAddress = "Test Address 123",
                    Notes = "Test order"
                };

                _context.Orders.Add(testOrder);
                await _context.SaveChangesAsync();

                return View("OrderCompleted", testOrder.Id);
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        private async Task<Product> GetProductFromDatabase(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
