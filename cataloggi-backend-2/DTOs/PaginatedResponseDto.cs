namespace cataloggi_backend_2.DTOs;

public class PaginatedResponseDto<T>
{
    public required List<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}