using System.ComponentModel.DataAnnotations;

namespace cataloggi_backend_2.Models;

public class Item
{
    public Guid Id { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1, MinimumLength = 1)]
    public string FirstLetter { get; set; } = string.Empty;

    [Required]
    [StringLength(4000, MinimumLength = 1)]
    public string Content { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }

    public Category Category { get; set; } = null!;
}
