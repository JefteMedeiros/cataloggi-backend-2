using cataloggi_backend_2.DTOs;

namespace cataloggi_backend_2.Services.Interfaces;

public interface IItemService
{
    Task<PaginatedResponseDto<ItemDto>> GetItems(int page, int pageSize, string search);
    Task<PaginatedResponseDto<ItemSummaryDto>> GetItemSummaries(int page, int pageSize, string search);
    Task<ItemDto> GetItem(Guid id);
    Task<ItemDto> CreateItem(CreateItemDto itemDto);
    Task<ItemDto> UpdateItem(Guid id, UpdateItemDto itemDto);
    Task DeleteItem(Guid id);
}