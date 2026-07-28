using cataloggi_backend_2.DTOs;

namespace cataloggi_backend_2.Helpers;

public static class PaginationHelper
{
    public static (int Page, int PageSize) Sanitize(int page, int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);
        return (page, pageSize);
    }

    public static PaginatedResponseDto<TDto> BuildResponse<T, TDto>(
        List<T> items,
        Func<T, TDto> mapper,
        int page,
        int pageSize,
        int totalItems)
    {
        return new PaginatedResponseDto<TDto>
        {
            Items = items.Select(mapper).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }
}
