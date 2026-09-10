namespace _003.Entregable03_EF.Domain;

public sealed class Orden
{
    public int IdOrden { get; set; }
    public int IdCliente { get; set; }
    public string Serie { get; set; } = string.Empty;
    public string Comprobante { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public bool State { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public Cliente Cliente { get; set; } = null!;
    public ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();
}
