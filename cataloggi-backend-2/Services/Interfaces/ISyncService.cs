using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.DTOs.Sync;

namespace cataloggi_backend_2.Services.Interfaces;

public interface ISyncService
{
    Task<CategorySyncResponseDto> SyncCategories(DateTime? since);
    Task<ItemSyncResponseDto> SyncItems(DateTime? since);
    Task<List<ItemDto>> GetItemsByIds(List<Guid> ids);
}
