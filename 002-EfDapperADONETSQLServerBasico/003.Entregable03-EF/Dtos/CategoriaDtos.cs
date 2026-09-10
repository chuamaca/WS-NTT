namespace _003.Entregable03_EF.Dtos;

public sealed record CategoriaDto(
    int IdCategoria,
    string Nombre,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy);

public sealed record CategoriaRequest(
    string Nombre,
    string Usuario);
