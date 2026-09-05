namespace BiblioNET.Consola.Models;

public sealed class Libro
{
    public int LibroId { get; init; }
    public int GeneroId { get; init; }
    public string Genero { get; init; } = string.Empty;
    public string Titulo { get; init; } = string.Empty;
    public string Autor { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
    public int Stock { get; init; }
    public bool Activo { get; init; }
}
