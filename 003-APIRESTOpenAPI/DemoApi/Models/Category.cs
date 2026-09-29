namespace DemoApi.Models;

/// <summary>
/// Representa una categoría para clasificar productos.
/// </summary>
public class Category
{
    /// <summary>
    /// Identificador único de la categoría.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre visible de la categoría.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción de la categoría.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}