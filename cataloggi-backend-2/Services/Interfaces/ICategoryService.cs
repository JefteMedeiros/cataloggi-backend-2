using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Category;

namespace cataloggi_backend_2.Services.Interfaces;

public interface ICategoryService
{
    Task<PaginatedResponseDto<CategoryDto>> GetCategories(int page, int pageSize, string search);
    Task<CategoryDto> GetCategory(Guid id);
    Task<CategoryDto> CreateCategory(CreateCategoryDto category);
    Task<CategoryDto> UpdateCategory(Guid id, UpdateCategoryDto category);
    Task DeleteCategory(Guid id);
}