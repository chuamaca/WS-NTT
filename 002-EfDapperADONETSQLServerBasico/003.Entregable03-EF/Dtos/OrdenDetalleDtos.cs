namespace _003.Entregable03_EF.Dtos;

public sealed record OrdenDetalleDto(
    int IdOrdenDetalles,
    int IdOrden,
    int IdProducto,
    string NombreProducto,
    int Cantidad,
    decimal Precio,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy);

public sealed record OrdenDetalleRequest(
    int IdOrden,
    int IdProducto,
    int Cantidad,
    decimal Precio,
    string Usuario);
