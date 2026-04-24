using cataloggi_backend_2.AppDbContext;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.Repositories;

public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
{
    public async Task<List<Category>> GetCategories()
    {
        return await context.Categories.ToListAsync();
    }
    
    public async Task<Category?> GetCategory(Guid id)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
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