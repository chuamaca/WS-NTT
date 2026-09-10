namespace _002.Entregable02_AdoNet.Model;

public sealed class OrdenCreateRequest
{
    public int IdCliente { get; init; }
    public string Serie { get; init; } = string.Empty;
    public string Comprobante { get; init; } = string.Empty;
    public string Usuario { get; init; } = string.Empty;
    public List<OrdenDetalleItem> Detalles { get; init; } = new();
}
