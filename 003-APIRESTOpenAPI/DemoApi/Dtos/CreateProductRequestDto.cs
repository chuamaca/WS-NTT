using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos;

/// <summary>
/// Datos necesarios para crear un producto.
/// </summary>
public class CreateProductRequestDto
{
    /// <summary>
    /// Nombre del producto.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Código único del producto.
    /// </summary>
    [Required(ErrorMessage = "Sku is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Sku must be between 3 and 50 characters.")]
    [RegularExpression("^[A-Za-z0-9-]+$", ErrorMessage = "Sku can only contain letters, numbers, and hyphens.")]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Precio del producto.
    /// </summary>
    [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0 and less than or equal to 1000000.")]
    public decimal Price { get; set; }

    /// <summary>
    /// Identificador de la categoría del producto.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0.")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Valor que indica si el producto está activo.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
