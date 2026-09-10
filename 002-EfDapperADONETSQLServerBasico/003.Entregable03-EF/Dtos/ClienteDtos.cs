namespace _003.Entregable03_EF.Dtos;

public sealed record ClienteDto(
    int IdCliente,
    string Nombre,
    string Apellido,
    string Email,
    string Telefono,
    string Direccion,
    string Documento,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy);

public sealed record ClienteRequest(
    string Nombre,
    string Apellido,
    string Email,
    string Telefono,
    string Direccion,
    string Documento,
    string Usuario);
