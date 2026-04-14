namespace cataloggi_backend_2.DTOs;

public class UpdateItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}