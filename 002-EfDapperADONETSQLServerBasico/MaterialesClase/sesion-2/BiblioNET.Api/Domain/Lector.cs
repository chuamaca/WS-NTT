namespace BiblioNET.Api.Domain;

public sealed class Lector
{
    public int LectorId { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; }

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
