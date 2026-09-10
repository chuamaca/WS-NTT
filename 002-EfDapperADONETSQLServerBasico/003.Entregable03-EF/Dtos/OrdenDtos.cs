namespace _003.Entregable03_EF.Dtos;

public sealed record OrdenDto(
    int IdOrden,
    int IdCliente,
    string NombreCliente,
    string ApellidoCliente,
    string Serie,
    string Comprobante,
    DateTime Fecha,
    decimal Total,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy);

public sealed record OrdenDetalleLineaDto(
    int IdOrdenDetalles,
    int IdProducto,
    string NombreProducto,
    int Cantidad,
    decimal Precio);

public sealed record OrdenConDetalleDto(
    int IdOrden,
    int IdCliente,
    string NombreCliente,
    string ApellidoCliente,
    string Serie,
    string Comprobante,
    DateTime Fecha,
    decimal Total,
    bool State,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? ModifiedAt,
    string? ModifiedBy,
    IReadOnlyList<OrdenDetalleLineaDto> Detalles);

public sealed record OrdenRequest(
    int IdCliente,
    string Serie,
    string Comprobante,
    decimal Total,
    string Usuario);

public sealed record OrdenDetalleItemRequest(
    int IdProducto,
    int Cantidad,
    decimal Precio);

public sealed record OrdenCreateRequest(
    int IdCliente,
    string Serie,
    string Comprobante,
    string Usuario,
    IReadOnlyList<OrdenDetalleItemRequest> Detalles);
