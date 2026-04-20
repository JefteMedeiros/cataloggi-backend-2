using cataloggi_backend_2.DTOs.Category;

namespace cataloggi_backend_2.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategories();
    Task<CategoryDto> GetCategory(Guid id);
    Task<CategoryDto> CreateCategory(CreateCategoryDto category);
    Task<CategoryDto> UpdateCategory(Guid id, UpdateCategoryDto category);
    Task DeleteCategory(Guid id);
}