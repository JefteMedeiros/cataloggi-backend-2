using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Category;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.Helpers;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private const int MaxCategoryNameLength = 64;

    public async Task<PaginatedResponseDto<CategoryDto>> GetCategories(int page, int pageSize, string search)
    {
        var (sanitizedPage, sanitizedPageSize) = PaginationHelper.Sanitize(page, pageSize);
        
        var categories = await categoryRepository.GetCategories(sanitizedPage, sanitizedPageSize, search);
        var totalItems = await categoryRepository.CountCategories(search);

        return PaginationHelper.BuildResponse(categories, MapToDto, sanitizedPage, sanitizedPageSize, totalItems);
    }

    public async Task<CategoryDto> GetCategory(Guid id)
    {
        var category = await categoryRepository.GetCategory(id);
        return MapToDto(category ?? throw new NotFoundException("Category not found"));
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryDto categoryDto)
    {
        var name = await GenerateUniqueName(categoryDto.Name.Trim());
        var slug = await GenerateUniqueSlug(name);

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        
        var createdCategory = await categoryRepository.CreateCategory(category);
        
        return MapToDto(createdCategory);
    }

    public async Task<CategoryDto> UpdateCategory(Guid id, UpdateCategoryDto categoryDto)
    {
        var categoryToEdit = await categoryRepository.GetCategory(id);

        if (categoryToEdit is null)
            throw new NotFoundException("Category not found");

        var name = categoryDto.Name.Trim();

        if (categoryToEdit.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            throw new ConflictException("Category name must be different from the current name");

        name = await GenerateUniqueName(name, categoryToEdit.Id);

        categoryToEdit.Name = name;
        categoryToEdit.Slug = await GenerateUniqueSlug(name, categoryToEdit.Id);
        
        await categoryRepository.UpdateCategory(categoryToEdit);
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

    private async Task<string> GenerateUniqueSlug(string name, Guid? excludedCategoryId = null)
    {
        var baseSlug = GenerateSlug(name);
        var slug = baseSlug;
        var suffix = 2;

        while (await categoryRepository.SlugExists(slug, excludedCategoryId))
        {
            slug = $"{baseSlug}-{suffix}";
            suffix++;
        }

        return slug;
    }

    private async Task<string> GenerateUniqueName(string name, Guid? excludedCategoryId = null)
    {
        var categoryName = name;
        var suffix = 2;

        while (await categoryRepository.NameExists(categoryName, excludedCategoryId))
        {
            var suffixText = $" {suffix}";
            var baseNameLength = MaxCategoryNameLength - suffixText.Length;
            var baseName = name.Length > baseNameLength
                ? name[..baseNameLength].TrimEnd()
                : name;

            categoryName = $"{baseName}{suffixText}";
            suffix++;
        }

        return categoryName;
    }

    private static string GenerateSlug(string value)
    {
        var normalizedValue = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (var character in normalizedValue)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character);

            if (category != UnicodeCategory.NonSpacingMark)
                builder.Append(character);
        }

        var slug = builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();

        slug = Regex.Replace(slug, @"[^a-z0-9]+", "-");
        slug = Regex.Replace(slug, @"-{2,}", "-").Trim('-');

        return string.IsNullOrWhiteSpace(slug) ? "category" : slug;
    }
}
