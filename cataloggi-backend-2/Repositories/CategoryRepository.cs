using cataloggi_backend_2.AppDbContext;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<List<Category>> GetCategories(int page, int pageSize, string search)
    {
        var query = context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(normalizedSearch));
        }

        return await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountCategories(string search)
    {
        var query = context.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(normalizedSearch));
        }

        return await query.CountAsync();
    }
    
    public async Task<Category?> GetCategory(Guid id)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> NameExists(string name, Guid? excludedCategoryId = null)
    {
        var normalizedName = name.ToLower();

        return await context.Categories.AnyAsync(c =>
            c.Name.ToLower() == normalizedName
            && (excludedCategoryId == null || c.Id != excludedCategoryId));
    }

    public async Task<bool> SlugExists(string slug, Guid? excludedCategoryId = null)
    {
        return await context.Categories.AnyAsync(c =>
            c.Slug == slug
            && (excludedCategoryId == null || c.Id != excludedCategoryId));
    }

    public async Task<Category> CreateCategory(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateCategory(Category category)
    {
        category.UpdatedAt = DateTime.UtcNow;
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category category)
    {
        category.DeletedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetCategoriesSince(DateTime since)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        return await context.Categories
            .IgnoreQueryFilters()
            .Where(c => c.UpdatedAt > since
                || (c.DeletedAt != null && c.DeletedAt > cutoffDate))
            .OrderBy(c => c.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<(List<Category> Items, int TotalCount)> GetCategoriesSincePaged(DateTime since, int page, int pageSize)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        var query = context.Categories
            .IgnoreQueryFilters()
            .Where(c => c.UpdatedAt > since
                || (c.DeletedAt != null && c.DeletedAt > cutoffDate));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.UpdatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Category> Items, int TotalCount)> GetAllCategoriesPaged(int page, int pageSize)
    {
        var query = context.Categories.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
