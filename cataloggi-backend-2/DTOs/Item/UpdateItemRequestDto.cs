using System.ComponentModel.DataAnnotations;

namespace cataloggi_backend_2.DTOs;

public class UpdateItemRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(128, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 128 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    [StringLength(4000, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 4000 characters")]
    public string Content { get; set; } = string.Empty;

    public string? CategoryId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(CategoryId))
        {
            yield return new ValidationResult("CategoryId is required", [nameof(CategoryId)]);
            yield break;
        }

        if (!Guid.TryParse(CategoryId, out _))
            yield return new ValidationResult("CategoryId must be a valid GUID", [nameof(CategoryId)]);
    }
}
