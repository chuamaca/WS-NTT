namespace _002.Entregable02_AdoNet.Model;

public sealed class ProductoRequest
{
    public int IdCategoria { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public decimal Precio { get; init; }
    public int Stock { get; init; }
    public string Usuario { get; init; } = string.Empty;
}
