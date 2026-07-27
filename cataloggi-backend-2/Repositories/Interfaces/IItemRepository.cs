using cataloggi_backend_2.Models;

namespace cataloggi_backend_2.Repositories.Interfaces;

public interface IItemRepository
{
    Task<List<Item>> GetItems(int page, int pageSize, string search);
    Task<int> CountItems(string search);
    Task<Item?> GetItem(Guid id);
    Task<Item> CreateItem(Item item);
    Task UpdateItem(Item item);
    Task DeleteItem(Item item);
    Task<List<Item>> GetItemsSince(DateTime since);
    Task<List<Item>> GetItemsByIds(List<Guid> ids);
    Task<(List<Item> Items, int TotalCount)> GetItemsSincePaged(DateTime since, int page, int pageSize);
}
