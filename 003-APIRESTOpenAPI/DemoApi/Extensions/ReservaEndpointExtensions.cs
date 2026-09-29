using DemoApi.Data;
using DemoApi.Dtos;
using DemoApi.Models;

namespace DemoApi.Extensions;

public static class ReservaEndpointExtensions
{
    public static IEndpointRouteBuilder MapReservaEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/reservas", (DateOnly? fecha, ReservaStore store) =>
        {
            var reservas = store.GetAll(fecha);
            return Results.Ok(new ApiResponse<List<Reserva>>
            {
                IsSuccess = true,
                Message = fecha is null
                    ? "Reservas obtenidas correctamente."
                    : $"Reservas del {fecha:yyyy-MM-dd} obtenidas correctamente.",
                Data = reservas
            });
        })
        .WithName("GetReservas")
        .WithTags("Reserva")
        .WithSummary("Obtiene todas las reservas o las filtra por fecha (yyyy-MM-dd).")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<List<Reserva>>>(StatusCodes.Status200OK);

        app.MapGet("/api/reservas/{id:int}", (int id, ReservaStore store) =>
        {
            var reserva = store.GetById(id);
            if (reserva is null)
            {
                return Results.NotFound(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "Reserva no encontrada.",
                    Data = null,
                    Errors = new List<string> { $"No existe una reserva con Id {id}." }
                });
            }

            return Results.Ok(new ApiResponse<Reserva>
            {
                IsSuccess = true,
                Message = "Reserva obtenida correctamente.",
                Data = reserva
            });
        })
        .WithName("GetReservaById")
        .WithTags("Reserva")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status404NotFound);

        app.MapPost("/api/reservas", (CreateReservaRequestDto request, ReservaStore store) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
            {
                return Results.BadRequest(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Data = null,
                    Errors = errors
                });
            }

            var nombreSala = request.NombreSala.Trim();
            if (store.HasOverlap(nombreSala, request.FechaReserva!.Value, request.HoraInicio!.Value, request.HoraFin!.Value))
                return Results.Conflict(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "La sala ya está reservada en ese horario.",
                    Data = null,
                    Errors = new List<string> { "Existe otra reserva activa para la misma sala que se cruza con el horario indicado." }
                });

            var reserva = store.Add(new Reserva
            {
                NombreSala = nombreSala,
                FechaReserva = request.FechaReserva.Value,
                HoraInicio = request.HoraInicio.Value,
                HoraFin = request.HoraFin.Value,
                NombreResponsable = request.NombreResponsable.Trim(),
                CantidadAsistentes = request.CantidadAsistentes,
                Motivo = request.Motivo.Trim(),
                Estado = EstadoReserva.Pendiente
            });

            return Results.Created($"/api/reservas/{reserva.Id}", new ApiResponse<Reserva>
            {
                IsSuccess = true,
                Message = "Reserva registrada correctamente.",
                Data = reserva,
                Errors = new List<string>()
            });
        })
        .WithName("CreateReserva")
        .WithTags("Reserva")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status201Created)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status409Conflict);

        app.MapPut("/api/reservas/{id:int}", (int id, UpdateReservaRequestDto request, ReservaStore store) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
            {
                return Results.BadRequest(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Data = null,
                    Errors = errors
                });
            }

            var reserva = store.GetById(id);
            if (reserva is null)
                return Results.NotFound(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "Reserva no encontrada.",
                    Data = null,
                    Errors = new List<string> { $"No existe una reserva con Id {id}." }
                });

            var nombreSala = request.NombreSala.Trim();
            if (request.Estado != EstadoReserva.Cancelada &&
                store.HasOverlap(nombreSala, request.FechaReserva!.Value, request.HoraInicio!.Value, request.HoraFin!.Value, excludeId: id))
                return Results.Conflict(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "La sala ya está reservada en ese horario.",
                    Data = null,
                    Errors = new List<string> { "Existe otra reserva activa para la misma sala que se cruza con el horario indicado." }
                });

            reserva.NombreSala = nombreSala;
            reserva.FechaReserva = request.FechaReserva!.Value;
            reserva.HoraInicio = request.HoraInicio!.Value;
            reserva.HoraFin = request.HoraFin!.Value;
            reserva.NombreResponsable = request.NombreResponsable.Trim();
            reserva.CantidadAsistentes = request.CantidadAsistentes;
            reserva.Motivo = request.Motivo.Trim();
            reserva.Estado = request.Estado!.Value;

            return Results.Ok(new ApiResponse<Reserva>
            {
                IsSuccess = true,
                Message = "Reserva actualizada correctamente.",
                Data = reserva,
                Errors = new List<string>()
            });
        })
        .WithName("UpdateReserva")
        .WithTags("Reserva")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status409Conflict);

        app.MapDelete("/api/reservas/{id:int}", (int id, bool? eliminar, ReservaStore store) =>
        {
            var reserva = store.GetById(id);
            if (reserva is null)
                return Results.NotFound(new ApiResponse<Reserva>
                {
                    IsSuccess = false,
                    Message = "Reserva no encontrada.",
                    Data = null,
                    Errors = new List<string> { $"No existe una reserva con Id {id}." }
                });

            if (eliminar == true)
            {
                store.Remove(id);
                return Results.NoContent();
            }

            reserva.Estado = EstadoReserva.Cancelada;
            return Results.Ok(new ApiResponse<Reserva>
            {
                IsSuccess = true,
                Message = "Reserva cancelada correctamente.",
                Data = reserva,
                Errors = new List<string>()
            });
        })
        .WithName("DeleteReserva")
        .WithTags("Reserva")
        .WithSummary("Cancela una reserva (estado Cancelada). Con ?eliminar=true la elimina definitivamente.")
        .RequireAuthorization()
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ApiResponse<Reserva>>(StatusCodes.Status404NotFound);

        return app;
    }
}
