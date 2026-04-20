using cataloggi_backend_2.DTOs.Category;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<List<CategoryDto>> GetCategories()
    {
        var categories = await categoryRepository.GetCategories();

        return categories.Select(MapToDto).ToList();
    }

    public async Task<CategoryDto> GetCategory(Guid id)
    {
        var category = await categoryRepository.GetCategory(id);
        return MapToDto(category ?? throw new NotFoundException("Category not found"));
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryDto categoryDto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = categoryDto.Name,
            Slug = Guid.NewGuid(),
        };
        
        var createdCategory = await categoryRepository.CreateCategory(category);
        
        return MapToDto(createdCategory);
    }

    public async Task<CategoryDto> UpdateCategory(Guid id, UpdateCategoryDto categoryDto)
    {
        var categoryToEdit = await categoryRepository.GetCategory(id);

        if (categoryToEdit is null)
            throw new NotFoundException("Category not found");

        if (categoryToEdit.Name.Equals(categoryDto.Name, StringComparison.InvariantCultureIgnoreCase))
            throw new ConflictException("Category name must be different from the current name");

        categoryToEdit.Name = categoryDto.Name;
        
        await categoryRepository.UpdateAsync(categoryToEdit);
        return MapToDto(categoryToEdit);
    }

    public async Task DeleteCategory(Guid id)
    {
        var categoryToDelete = await categoryRepository.GetCategory(id);

        if (categoryToDelete is null)
            throw new NotFoundException("Category not found");
        
        await categoryRepository.DeleteCategory(categoryToDelete);
    }
    
    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
        };
    }
}