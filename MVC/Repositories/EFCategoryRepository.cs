namespace THLapTrinhWeb.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using THLapTrinhWeb.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                return _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<Category> GetByIdAsync(int id)
        {
            return _context.Categories.FindAsync(id).AsTask();
        }

        public Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            return _context.SaveChangesAsync();
        }
    }
}