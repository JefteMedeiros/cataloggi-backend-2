namespace cataloggi_backend_2.DTOs.Sync;

public class ItemSyncResponseDto
{
    public List<ItemSyncItemDto> Updated { get; set; } = [];
    public List<Guid> Deleted { get; set; } = [];
    public DateTime SyncedAt { get; set; }
}

public class ItemSyncItemDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FirstLetter { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
