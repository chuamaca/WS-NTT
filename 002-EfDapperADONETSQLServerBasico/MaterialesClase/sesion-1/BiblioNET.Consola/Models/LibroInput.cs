namespace BiblioNET.Consola.Models;

public sealed class LibroInput
{
    public int GeneroId { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Autor { get; init; } = string.Empty;
    public string ISBN { get; init; } = string.Empty;
    public int Stock { get; init; }
}
