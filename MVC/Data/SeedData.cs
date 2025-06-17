using Microsoft.EntityFrameworkCore;
using THLapTrinhWeb.Models;

namespace THLapTrinhWeb.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Kiểm tra xem đã có data chưa
            if (context.Categories.Any() || context.Products.Any())
            {
                return; // Đã có data rồi
            }

            // Seed Categories
            await SeedCategories(context);

            // Seed Products  
            await SeedProducts(context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedCategories(ApplicationDbContext context)
        {
            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Fashion" },
                new Category { Name = "Home & Garden" },
                new Category { Name = "Sports" },
                new Category { Name = "Books" },
                new Category { Name = "Health & Beauty" },
                new Category { Name = "Toys & Games" },
                new Category { Name = "Automotive" },
                new Category { Name = "Food & Beverages" },
                new Category { Name = "Office Supplies" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProducts(ApplicationDbContext context)
        {
            var categories = await context.Categories.ToListAsync();

            var products = new List<Product>
            {
                // Electronics
                new Product
                {
                    Name = "iPhone 15 Pro",
                    Price = 25990000,
                    Description = "Latest iPhone with A17 Pro chip and titanium design",
                    ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/305658/iphone-15-pro-natural-titanium-1-2.jpg",
                    CategoryId = categories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "Samsung Galaxy S24 Ultra",
                    Price = 32990000,
                    Description = "Premium Android phone with S Pen and AI features",
                    ImageUrl = "https://cdn.tgdd.vn/Products/Images/42/307174/samsung-galaxy-s24-ultra-grey-1-2.jpg",
                    CategoryId = categories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "MacBook Air M3",
                    Price = 32990000,
                    Description = "Lightweight laptop with M3 chip for everyday computing",
                    ImageUrl = "https://cdn.tgdd.vn/Products/Images/44/322096/macbook-air-13-inch-m3-2024-starlight-1-2.jpg",
                    CategoryId = categories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "Dell XPS 13",
                    Price = 28990000,
                    Description = "Premium Windows laptop with stunning display",
                    ImageUrl = "https://i.dell.com/is/image/DellContent/content/dam/ss2/product-images/dell-client-products/notebooks/xps-notebooks/xps-13-9315/media-gallery/silver/notebook-xps-13-9315-sl-gallery-1.psd",
                    CategoryId = categories.First(c => c.Name == "Electronics").Id
                },
                new Product
                {
                    Name = "AirPods Pro 2",
                    Price = 6490000,
                    Description = "Premium wireless earbuds with active noise cancellation",
                    ImageUrl = "https://cdn.tgdd.vn/Products/Images/54/289780/tai-nghe-bluetooth-airpods-pro-2nd-gen-usb-c-apple-mmt73-1-2.jpg",
                    CategoryId = categories.First(c => c.Name == "Electronics").Id
                },

                // Fashion
                new Product
                {
                    Name = "Nike Air Force 1",
                    Price = 2999000,
                    Description = "Classic basketball sneakers with timeless style",
                    ImageUrl = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/b7d9211c-26e7-431a-ac24-b0540fb3c00f/air-force-1-07-shoes-WrLlWX.png",
                    CategoryId = categories.First(c => c.Name == "Fashion").Id
                },
                new Product
                {
                    Name = "Adidas Ultraboost 22",
                    Price = 4599000,
                    Description = "High-performance running shoes with boost technology",
                    ImageUrl = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/fbaf991a78bc4896a3e9ad7800abcec6_9366/Ultraboost_22_Shoes_Black_GZ0127_01_standard.jpg",
                    CategoryId = categories.First(c => c.Name == "Fashion").Id
                },
                new Product
                {
                    Name = "Levi's 501 Original Jeans",
                    Price = 1899000,
                    Description = "Classic straight leg jeans with authentic wash",
                    ImageUrl = "https://lsco.scene7.com/is/image/lsco/005010000-front-pdp-lse",
                    CategoryId = categories.First(c => c.Name == "Fashion").Id
                },
                new Product
                {
                    Name = "Uniqlo Heattech T-Shirt",
                    Price = 399000,
                    Description = "Thermal underwear with heat retention technology",
                    ImageUrl = "https://image.uniqlo.com/UQ/ST3/AsianCommon/imagesgoods/455359/item/goods_09_455359.jpg",
                    CategoryId = categories.First(c => c.Name == "Fashion").Id
                },

                // Home & Garden
                new Product
                {
                    Name = "IKEA Sofa Bed",
                    Price = 12990000,
                    Description = "Comfortable sofa that converts to bed",
                    ImageUrl = "https://www.ikea.com/us/en/images/products/friheten-sleeper-sofa-skiftebo-dark-gray__0175610_pe328883_s5.jpg",
                    CategoryId = categories.First(c => c.Name == "Home & Garden").Id
                },
                new Product
                {
                    Name = "Dyson V15 Detect",
                    Price = 18990000,
                    Description = "Cordless vacuum with laser dust detection",
                    ImageUrl = "https://dyson-h.assetsadobe2.com/is/image/content/dam/dyson/products/vacuum-cleaners/V15/detect/gold/dyson-v15-detect-absolute-gold-01.jpg",
                    CategoryId = categories.First(c => c.Name == "Home & Garden").Id
                },
                new Product
                {
                    Name = "Philips Air Fryer",
                    Price = 3990000,
                    Description = "Healthy cooking with little to no oil",
                    ImageUrl = "https://images.philips.com/is/image/philipsconsumer/2f9b2bb70ec846b6a1a7b2bc016c7e57",
                    CategoryId = categories.First(c => c.Name == "Home & Garden").Id
                },

                // Sports
                new Product
                {
                    Name = "Yoga Mat Premium",
                    Price = 899000,
                    Description = "Non-slip yoga mat with extra cushioning",
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0449/5225/6667/products/6mmyogamat-purple-1_1200x1200.jpg",
                    CategoryId = categories.First(c => c.Name == "Sports").Id
                },
                new Product
                {
                    Name = "Dumbbells Set 20kg",
                    Price = 2490000,
                    Description = "Adjustable dumbbells for home workout",
                    ImageUrl = "https://m.media-amazon.com/images/I/71QzB2QoEAL._AC_SL1500_.jpg",
                    CategoryId = categories.First(c => c.Name == "Sports").Id
                },
                new Product
                {
                    Name = "Treadmill Foldable",
                    Price = 15990000,
                    Description = "Compact treadmill perfect for home use",
                    ImageUrl = "https://www.horizonfitness.com/on/demandware.static/-/Sites-horizon-master-catalog/default/dw0c7f5a3f/images/7.0-AT/7.0AT-Console.jpg",
                    CategoryId = categories.First(c => c.Name == "Sports").Id
                },

                // Books
                new Product
                {
                    Name = "The Psychology of Money",
                    Price = 299000,
                    Description = "Timeless lessons on wealth, greed, and happiness",
                    ImageUrl = "https://m.media-amazon.com/images/I/71g2ednj0JL._AC_UF1000,1000_QL80_.jpg",
                    CategoryId = categories.First(c => c.Name == "Books").Id
                },
                new Product
                {
                    Name = "Atomic Habits",
                    Price = 349000,
                    Description = "An easy & proven way to build good habits",
                    ImageUrl = "https://m.media-amazon.com/images/I/81wgcld4wxL._AC_UF1000,1000_QL80_.jpg",
                    CategoryId = categories.First(c => c.Name == "Books").Id
                },

                // Health & Beauty
                new Product
                {
                    Name = "Cetaphil Gentle Cleanser",
                    Price = 389000,
                    Description = "Gentle daily cleanser for all skin types",
                    ImageUrl = "https://www.cetaphil.com/dw/image/v2/BBPQ_PRD/on/demandware.static/-/Sites-galderma-master-catalog/default/dwa6a7a5f8/images/large/302993901309-1.jpg",
                    CategoryId = categories.First(c => c.Name == "Health & Beauty").Id
                },
                new Product
                {
                    Name = "Olay Regenerist Serum",
                    Price = 649000,
                    Description = "Anti-aging serum with niacinamide",
                    ImageUrl = "https://images.ulta.com/is/image/Ulta/2267334",
                    CategoryId = categories.First(c => c.Name == "Health & Beauty").Id
                },

                // Toys & Games
                new Product
                {
                    Name = "LEGO Creator Expert",
                    Price = 4999000,
                    Description = "Advanced building set for adults",
                    ImageUrl = "https://www.lego.com/cdn/cs/set/assets/blt4c5d4b6d5b0e3d85/10264.jpg",
                    CategoryId = categories.First(c => c.Name == "Toys & Games").Id
                },
                new Product
                {
                    Name = "Nintendo Switch OLED",
                    Price = 8990000,
                    Description = "Portable gaming console with OLED screen",
                    ImageUrl = "https://assets.nintendo.com/image/upload/f_auto/q_auto/dpr_2.0/c_scale,w_500/ncom/en_US/switch/site-design-update/hardware/switch-oled/gallery/image01",
                    CategoryId = categories.First(c => c.Name == "Toys & Games").Id
                },

                // Automotive
                new Product
                {
                    Name = "Car Phone Mount",
                    Price = 299000,
                    Description = "Magnetic phone holder for car dashboard",
                    ImageUrl = "https://m.media-amazon.com/images/I/61QYZvZJ7HL._AC_SL1500_.jpg",
                    CategoryId = categories.First(c => c.Name == "Automotive").Id
                },
                new Product
                {
                    Name = "Car Air Freshener",
                    Price = 149000,
                    Description = "Long lasting vanilla scent for car",
                    ImageUrl = "https://m.media-amazon.com/images/I/61gK8ZE3ZxL._AC_SL1500_.jpg",
                    CategoryId = categories.First(c => c.Name == "Automotive").Id
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        // Method để lấy sample data cho testing
        // public static List<Category> GetSampleCategories()
        // {
        //     return new List<Category>
        //     {
        //         new Category { Id = 1, Name = "Electronics" },
        //         new Category { Id = 2, Name = "Fashion" },
        //         new Category { Id = 3, Name = "Home & Garden" },
        //         new Category { Id = 4, Name = "Sports" },
        //         new Category { Id = 5, Name = "Books" },
        //         new Category { Id = 6, Name = "Health & Beauty" },
        //         new Category { Id = 7, Name = "Toys & Games" },
        //         new Category { Id = 8, Name = "Automotive" }
        //     };
        // }

        // public static List<Product> GetSampleProducts()
        // {
        //     return new List<Product>
        //     {
        //         new Product { Id = 1, Name = "iPhone 15 Pro", Price = 25990000, Description = "Latest iPhone", CategoryId = 1 },
        //         new Product { Id = 2, Name = "Samsung Galaxy S24", Price = 32990000, Description = "Premium Android", CategoryId = 1 },
        //         new Product { Id = 3, Name = "Nike Air Force 1", Price = 2999000, Description = "Classic sneakers", CategoryId = 2 },
        //         new Product { Id = 4, Name = "Adidas Ultraboost", Price = 4599000, Description = "Running shoes", CategoryId = 2 },
        //         new Product { Id = 5, Name = "IKEA Sofa", Price = 12990000, Description = "Comfortable sofa", CategoryId = 3 }
        //     };
        // }
    }
}