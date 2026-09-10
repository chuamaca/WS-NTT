namespace _002.Entregable02_AdoNet.Model;

public sealed class Orden
{
    public int IdOrden { get; init; }
    public int IdCliente { get; init; }
    public string NombreCliente { get; init; } = string.Empty;
    public string ApellidoCliente { get; init; } = string.Empty;
    public string Serie { get; init; } = string.Empty;
    public string Comprobante { get; init; } = string.Empty;
    public DateTime Fecha { get; init; }
    public decimal Total { get; init; }
    public bool State { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime? ModifiedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public bool IsDeleted { get; init; }
    public List<OrdenDetalleLinea> Detalles { get; init; } = new();
}
