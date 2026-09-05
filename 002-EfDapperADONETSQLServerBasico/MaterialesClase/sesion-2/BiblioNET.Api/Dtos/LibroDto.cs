namespace BiblioNET.Api.Dtos;

public sealed record LibroDto(
    int LibroId,
    int GeneroId,
    string Genero,
    string Titulo,
    string Autor,
    string ISBN,
    int Stock);
