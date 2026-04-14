namespace cataloggi_backend_2.DTOs;

public class CreateItemDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}