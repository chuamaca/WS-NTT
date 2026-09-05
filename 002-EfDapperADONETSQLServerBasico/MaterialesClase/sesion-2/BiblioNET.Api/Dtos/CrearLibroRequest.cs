namespace BiblioNET.Api.Dtos;

public sealed record CrearLibroRequest(
    int GeneroId,
    string Titulo,
    string Autor,
    string ISBN,
    int Stock);