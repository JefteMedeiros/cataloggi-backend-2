using cataloggi_backend_2.Models;

namespace cataloggi_backend_2.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategories();
    Task<Category?> GetCategory(Guid id);
    Task<bool> NameExists(string name, Guid? excludedCategoryId = null);
    Task<bool> SlugExists(string slug, Guid? excludedCategoryId = null);
    Task<Category> CreateCategory(Category category);
    Task UpdateCategory(Category category);
    Task DeleteCategory(Category category);
}
