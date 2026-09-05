namespace BiblioNET.Api.Domain;

public sealed class Prestamo
{
    public int PrestamoId { get; set; }
    public int LectorId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; } = "ACTIVO";

    public Lector Lector { get; set; } = null!;
    public ICollection<PrestamoDetalle> Detalles { get; set; } = new List<PrestamoDetalle>();
}
