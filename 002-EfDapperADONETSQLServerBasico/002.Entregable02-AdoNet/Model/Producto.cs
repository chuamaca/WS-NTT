namespace _002.Entregable02_AdoNet.Model;

public sealed class Producto
{
    public int IdProducto { get; init; }
    public int IdCategoria { get; init; }
    public string NombreCategoria { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public decimal Precio { get; init; }
    public int Stock { get; init; }
    public bool State { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? ModifiedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public bool IsDeleted { get; init; }
}
