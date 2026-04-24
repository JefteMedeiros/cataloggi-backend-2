using System.ComponentModel.DataAnnotations;

namespace cataloggi_backend_2.DTOs.Category;

public class UpdateCategoryDto : IValidatableObject
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(64, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 64 characters")]
    public string Name { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult("Name cannot be empty", [nameof(Name)]);
    }
}
