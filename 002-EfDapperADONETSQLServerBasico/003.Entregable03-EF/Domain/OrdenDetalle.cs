namespace _003.Entregable03_EF.Domain;

public sealed class OrdenDetalle
{
    public int IdOrdenDetalles { get; set; }
    public int IdOrden { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public bool State { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public Orden Orden { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
