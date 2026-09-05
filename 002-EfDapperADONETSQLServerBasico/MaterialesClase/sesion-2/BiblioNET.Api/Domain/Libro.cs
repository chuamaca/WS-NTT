namespace BiblioNET.Api.Domain;

public sealed class Libro
{
    public int LibroId { get; set; }
    public int GeneroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int Stock { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; }

    public Genero Genero { get; set; } = null!;
    public ICollection<PrestamoDetalle> PrestamoDetalles { get; set; } = new List<PrestamoDetalle>();
}
