namespace cataloggi_backend_2.DTOs;

public class ItemSummaryDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FirstLetter { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}