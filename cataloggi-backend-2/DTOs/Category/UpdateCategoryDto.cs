namespace cataloggi_backend_2.DTOs.Category;
using System.ComponentModel.DataAnnotations;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(64, ErrorMessage = "Name must be at most 64 characters")]
    public string Name { get; set; } = string.Empty;
}