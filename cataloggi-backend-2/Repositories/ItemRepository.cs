using cataloggi_backend_2.AppDbContext;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.Repositories;

public class ItemRepository(ApplicationDbContext context): IItemRepository
{
    public async Task<List<Item>> GetItems()
    {
        return await context.Items.ToListAsync();
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
        context.Items.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteItem(Item item)
    {
        context.Items.Remove(item);
        await context.SaveChangesAsync();
    }
}