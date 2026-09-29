using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos;

/// <summary>
/// Datos necesarios para crear una categoría.
/// </summary>
public class CreateCategoryRequestDto
{
    /// <summary>
    /// Nombre visible de la categoría.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la categoría.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;
}