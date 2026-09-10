namespace _002.Entregable02_AdoNet.Model;

public sealed class OrdenDetalle
{
    public int IdOrdenDetalles { get; init; }
    public int IdOrden { get; init; }
    public int IdProducto { get; init; }
    public string NombreProducto { get; init; } = string.Empty;
    public int Cantidad { get; init; }
    public decimal Precio { get; init; }
    public bool State { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? ModifiedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public bool IsDeleted { get; init; }
}
