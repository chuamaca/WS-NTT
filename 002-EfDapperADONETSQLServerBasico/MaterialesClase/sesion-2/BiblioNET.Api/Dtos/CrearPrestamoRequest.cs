namespace BiblioNET.Api.Dtos;

public sealed record CrearPrestamoRequest(
    int LectorId,
    int LibroId,
    short Cantidad,
    int DiasPrestamo = 7);
