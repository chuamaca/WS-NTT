namespace _002.Entregable02_AdoNet.Model;

public sealed class OrdenDetalleRequest
{
    public int IdOrden { get; init; }
    public int IdProducto { get; init; }
    public int Cantidad { get; init; }
    public decimal Precio { get; init; }
    public string Usuario { get; init; } = string.Empty;
}
