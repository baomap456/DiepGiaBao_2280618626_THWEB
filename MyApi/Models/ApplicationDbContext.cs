using Microsoft.EntityFrameworkCore;
using MyApi.Models;
public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
}