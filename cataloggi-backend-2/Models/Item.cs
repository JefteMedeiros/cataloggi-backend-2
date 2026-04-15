namespace cataloggi_backend_2.Models;

public class Item
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FirstLetter { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }

    public Category Category { get; set; } = null!;
}