namespace _002.Entregable02_AdoNet.Model;

public sealed class OrdenDetalleLinea
{
    public int IdOrdenDetalles { get; init; }
    public int IdProducto { get; init; }
    public string NombreProducto { get; init; } = string.Empty;
    public int Cantidad { get; init; }
    public decimal Precio { get; init; }
}
