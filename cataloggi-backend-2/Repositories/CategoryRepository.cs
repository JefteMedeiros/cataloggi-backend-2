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
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category category)
    {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }
}
