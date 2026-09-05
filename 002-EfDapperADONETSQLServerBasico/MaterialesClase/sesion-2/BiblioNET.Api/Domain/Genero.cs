namespace BiblioNET.Api.Domain;

public sealed class Genero
{
    public int GeneroId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; }

    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
