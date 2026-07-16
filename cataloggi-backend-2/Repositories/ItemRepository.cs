using cataloggi_backend_2.AppDbContext;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.Repositories;

public class ItemRepository(ApplicationDbContext context): IItemRepository
{
    public async Task<List<Item>> GetItems(int page, int pageSize, string search)
    {
        var query = context.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.ToLower();
            query = query.Where(i =>
                i.Name.ToLower().Contains(normalizedSearch) ||
                i.Content.ToLower().Contains(normalizedSearch));
        }

        return await query
            .OrderBy(i => i.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountItems(string search)
    {
        var query = context.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.ToLower();
            query = query.Where(i =>
                i.Name.ToLower().Contains(normalizedSearch) ||
                i.Content.ToLower().Contains(normalizedSearch));
        }

        return await query.CountAsync();
    }
    
    public async Task<Item?> GetItem(Guid id)
    {
        return await context.Items.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Item> CreateItem(Item item)
    {
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateItem(Item item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        context.Items.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteItem(Item item)
    {
        item.DeletedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        context.Items.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<Item>> GetItemsSince(DateTime since)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        return await context.Items
            .IgnoreQueryFilters()
            .Where(i => i.UpdatedAt > since
                || (i.DeletedAt != null && i.DeletedAt > cutoffDate))
            .OrderBy(i => i.UpdatedAt)
            .ToListAsync();
    }

    public async Task<List<Item>> GetItemsByIds(List<Guid> ids)
    {
        return await context.Items
            .Where(i => ids.Contains(i.Id))
            .ToListAsync();
    }
}
