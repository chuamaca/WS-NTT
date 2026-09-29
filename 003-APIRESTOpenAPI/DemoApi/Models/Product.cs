namespace DemoApi.Models;

/// <summary>
/// Representa un producto disponible en el catálogo.
/// </summary>
public class Product
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre visible del producto.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Código único del producto.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Precio del producto.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Identificador de la categoría del producto.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Categoría a la que pertenece el producto.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Indica si el producto está activo.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
