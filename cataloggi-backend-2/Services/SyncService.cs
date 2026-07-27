using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Sync;
using cataloggi_backend_2.Repositories.Interfaces;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Services;

public class SyncService(
    ICategoryRepository categoryRepository,
    IItemRepository itemRepository) : ISyncService
{
    public async Task<CategorySyncResponseDto> SyncCategories(DateTime? since, int page, int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 1000);

        var (categories, totalCount) = since.HasValue
            ? await categoryRepository.GetCategoriesSincePaged(since.Value, page, pageSize)
            : await categoryRepository.GetAllCategoriesPaged(page, pageSize);

        var syncedAt = DateTime.UtcNow;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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
            SyncedAt = syncedAt,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalItems = totalCount
        };
    }

    public async Task<ItemSyncResponseDto> SyncItems(DateTime? since, int page, int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 1000);

        var (items, totalCount) = since.HasValue
            ? await itemRepository.GetItemsSincePaged(since.Value, page, pageSize)
            : await itemRepository.GetItemsSincePaged(DateTime.MinValue, page, pageSize);

        var syncedAt = DateTime.UtcNow;
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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
            SyncedAt = syncedAt,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalItems = totalCount
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
