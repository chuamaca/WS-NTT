namespace BiblioNET.Api.Dtos;

public sealed record LibroRequest(
    int GeneroId,
    string Titulo,
    string Autor,
    string ISBN,
    int Stock);
