using cataloggi_backend_2.Models;

namespace cataloggi_backend_2.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategories();
    Task<Category?> GetCategory(Guid id);
    Task<Category> CreateCategory(Category category);
    Task UpdateAsync(Category category);
    Task DeleteCategory(Category category);
}
