using System.ComponentModel.DataAnnotations;

namespace cataloggi_backend_2.Models;

public class Category
{
    public Guid Id { get; set; }
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;
    public Guid Slug { get; set; }
}