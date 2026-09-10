namespace _003.Entregable03_EF.Dtos;

public sealed record ProductoDto(
    int IdProducto,
    int IdCategoria,
    string NombreCategoria,
    string Nombre,
    decimal Precio,
    int Stock,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy);

public sealed record ProductoRequest(
    int IdCategoria,
    string Nombre,
    decimal Precio,
    int Stock,
    string Usuario);
