using cataloggi_backend_2.Models;

namespace cataloggi_backend_2.Repositories.Interfaces;

public interface IItemRepository
{
    Task<List<Item>> GetItems();
    Task<Item?> GetItem(Guid id);
    Task<Item> CreateItem(Item item);
    Task UpdateItem(Item item);
    Task DeleteItem(Item item);
}