namespace THLapTrinhWeb.Repositories
{
    using THLapTrinhWeb.Models;
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}