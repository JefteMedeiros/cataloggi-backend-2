using System.ComponentModel.DataAnnotations;

namespace cataloggi_backend_2.Models;

public class Category
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(96, MinimumLength = 1)]
    public string Slug { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
