using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Sync;
using cataloggi_backend_2.Repositories.Interfaces;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Services;

public class SyncService(
    ICategoryRepository categoryRepository,
    IItemRepository itemRepository) : ISyncService
{
    public async Task<CategorySyncResponseDto> SyncCategories(DateTime? since)
    {
        var categories = since.HasValue
            ? await categoryRepository.GetCategoriesSince(since.Value)
            : await categoryRepository.GetAllCategories();

        var syncedAt = DateTime.UtcNow;

        var updated = categories
            .Where(c => c.DeletedAt == null)
            .Select(c => new CategorySyncItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToList();

        var deleted = categories
            .Where(c => c.DeletedAt != null && (!since.HasValue || c.DeletedAt > since))
            .Select(c => c.Id)
            .ToList();

        return new CategorySyncResponseDto
        {
            Updated = updated,
            Deleted = deleted,
            SyncedAt = syncedAt
        };
    }

    public async Task<ItemSyncResponseDto> SyncItems(DateTime? since)
    {
        var items = since.HasValue
            ? await itemRepository.GetItemsSince(since.Value)
            : await itemRepository.GetItemsByIds([]);

        var syncedAt = DateTime.UtcNow;

        var updated = items
            .Where(i => i.DeletedAt == null)
            .Select(i => new ItemSyncItemDto
            {
                Id = i.Id,
                CategoryId = i.CategoryId,
                Name = i.Name,
                FirstLetter = i.FirstLetter,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            })
            .ToList();

        var deleted = items
            .Where(i => i.DeletedAt != null && (!since.HasValue || i.DeletedAt > since))
            .Select(i => i.Id)
            .ToList();

        return new ItemSyncResponseDto
        {
            Updated = updated,
            Deleted = deleted,
            SyncedAt = syncedAt
        };
    }

    public async Task<List<ItemDto>> GetItemsByIds(List<Guid> ids)
    {
        var items = await itemRepository.GetItemsByIds(ids);

        return items.Select(i => new ItemDto
        {
            Id = i.Id,
            CategoryId = i.CategoryId,
            Name = i.Name,
            FirstLetter = i.FirstLetter,
            Content = i.Content,
            UpdatedAt = i.UpdatedAt
        }).ToList();
    }
}
