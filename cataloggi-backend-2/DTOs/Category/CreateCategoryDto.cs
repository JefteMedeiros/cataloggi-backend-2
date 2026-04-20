using System.ComponentModel.DataAnnotations;
namespace cataloggi_backend_2.DTOs.Category;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(64, ErrorMessage = "Name must be at most 64 characters")]
    public string Name { get; set; } = string.Empty;
}