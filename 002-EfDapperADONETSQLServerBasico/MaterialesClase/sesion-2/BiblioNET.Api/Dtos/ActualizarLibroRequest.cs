namespace BiblioNET.Api.Dtos;

public sealed record ActualizarLibroRequest(
    int GeneroId,
    string Titulo,
    string Autor,
    string ISBN,
    int Stock);