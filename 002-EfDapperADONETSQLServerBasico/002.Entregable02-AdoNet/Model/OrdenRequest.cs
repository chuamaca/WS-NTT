namespace _002.Entregable02_AdoNet.Model;

public sealed class OrdenRequest
{
    public int IdCliente { get; init; }
    public string Serie { get; init; } = string.Empty;
    public string Comprobante { get; init; } = string.Empty;
    public decimal Total { get; init; }
    public string Usuario { get; init; } = string.Empty;
}
