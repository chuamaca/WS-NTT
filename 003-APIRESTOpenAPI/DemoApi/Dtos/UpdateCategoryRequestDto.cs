using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos;

/// <summary>
/// Datos que pueden modificarse de una categoría existente.
/// </summary>
public class UpdateCategoryRequestDto
{
    /// <summary>
    /// Nuevo nombre de la categoría.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Nueva descripción de la categoría.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;
}