namespace BiblioNET.Api.Domain;

public sealed class PrestamoDetalle
{
    public int PrestamoDetalleId { get; set; }
    public int PrestamoId { get; set; }
    public int LibroId { get; set; }
    public short Cantidad { get; set; } = 1;
    public bool Devuelto { get; set; }

    public Prestamo Prestamo { get; set; } = null!;
    public Libro Libro { get; set; } = null!;
}
