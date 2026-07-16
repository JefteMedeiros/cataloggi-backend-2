namespace cataloggi_backend_2.DTOs.Sync;

public class CategorySyncResponseDto
{
    public List<CategorySyncItemDto> Updated { get; set; } = [];
    public List<Guid> Deleted { get; set; } = [];
    public DateTime SyncedAt { get; set; }
}

public class CategorySyncItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
