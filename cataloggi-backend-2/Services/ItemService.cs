using cataloggi_backend_2.DTOs;
using cataloggi_backend_2.Exceptions;
using cataloggi_backend_2.Helpers;
using cataloggi_backend_2.Models;
using cataloggi_backend_2.Repositories.Interfaces;
using cataloggi_backend_2.Services.Interfaces;

namespace cataloggi_backend_2.Services;

public class ItemService(IItemRepository itemRepository, ICategoryRepository categoryRepository) : IItemService
{
    public async Task<PaginatedResponseDto<ItemDto>> GetItems(int page, int pageSize, string search)
    {
        var (sanitizedPage, sanitizedPageSize) = PaginationHelper.Sanitize(page, pageSize);

        var items = await itemRepository.GetItems(sanitizedPage, sanitizedPageSize, search);
        var totalItems = await itemRepository.CountItems(search);

        return PaginationHelper.BuildResponse(items, MapToDto, sanitizedPage, sanitizedPageSize, totalItems);
    }

    public async Task<PaginatedResponseDto<ItemSummaryDto>> GetItemSummaries(int page, int pageSize, string search)
    {
        var (sanitizedPage, sanitizedPageSize) = PaginationHelper.Sanitize(page, pageSize);

        var items = await itemRepository.GetItems(sanitizedPage, sanitizedPageSize, search);
        var totalItems = await itemRepository.CountItems(search);

        return PaginationHelper.BuildResponse(items, MapToSummaryDto, sanitizedPage, sanitizedPageSize, totalItems);
    }

    public async Task<ItemDto> GetItem(Guid id)
    {
        var item = await itemRepository.GetItem(id);
        
        return MapToDto(item ?? throw new NotFoundException("The requested item was not found."));
    }

    public async Task<ItemDto> CreateItem(CreateItemDto itemDto)
    {
        await EnsureCategoryExists(itemDto.CategoryId);

        var name = itemDto.Name.Trim();
        
        var item = new Item
        {
            CategoryId = itemDto.CategoryId,
            Name = name,
            FirstLetter = GetFirstLetter(name),
            Content = itemDto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var createdItem = await itemRepository.CreateItem(item);
        
        return MapToDto(createdItem);
    }

    public async Task<ItemDto> UpdateItem(Guid id, UpdateItemDto itemDto)
    {
        var itemToEdit = await itemRepository.GetItem(id);
        
        if (itemToEdit is null)
            throw new NotFoundException("The requested item was not found.");

        await EnsureCategoryExists(itemDto.CategoryId);

        var name = itemDto.Name.Trim();
        var content = itemDto.Content ?? itemToEdit.Content;
        
        if (itemToEdit.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
            && itemToEdit.CategoryId == itemDto.CategoryId
            && itemToEdit.Content.Equals(content, StringComparison.InvariantCultureIgnoreCase))
            throw new ConflictException("Item data must be different from the current data.");
        
        itemToEdit.CategoryId = itemDto.CategoryId;
        itemToEdit.Name = name;
        itemToEdit.FirstLetter = GetFirstLetter(name);
        itemToEdit.Content = content;
        itemToEdit.UpdatedAt = DateTime.UtcNow;
        
        await itemRepository.UpdateItem(itemToEdit);
        return MapToDto(itemToEdit);
    }

    public async Task DeleteItem(Guid id)
    {
        var itemToDelete = await itemRepository.GetItem(id);
        
        if (itemToDelete is null)
            throw new NotFoundException("The requested item was not found.");

        await itemRepository.DeleteItem(itemToDelete);
    }

    private static string GetFirstLetter(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ConflictException("Item name cannot be empty.");

        return name.Trim()[0].ToString().ToUpperInvariant();
    }

    private async Task EnsureCategoryExists(Guid categoryId)
    {
        var category = await categoryRepository.GetCategory(categoryId);

        if (category is null)
            throw new BadRequestException("CategoryId does not reference an existing category.");
    }

    private static ItemSummaryDto MapToSummaryDto(Item item)
    {
        return new ItemSummaryDto
        {
            Id = item.Id,
            CategoryId = item.CategoryId,
            Name = item.Name,
            FirstLetter = item.FirstLetter,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
    
    private static ItemDto MapToDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            CategoryId = item.CategoryId,
            Name = item.Name,
            FirstLetter = item.FirstLetter,
            Content = item.Content,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
        };
    }
}
