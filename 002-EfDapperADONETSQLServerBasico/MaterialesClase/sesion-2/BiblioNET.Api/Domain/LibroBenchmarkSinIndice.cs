namespace BiblioNET.Api.Domain;

public sealed class LibroBenchmarkSinIndice
{
    public int LibroBenchmarkSinIndiceId { get; set; }
    public int GeneroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int Stock { get; set; }
    public bool Activo { get; set; } = true;
}
