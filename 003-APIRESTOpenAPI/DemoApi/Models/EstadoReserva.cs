using System.Text.Json.Serialization;

namespace DemoApi.Models;

/// <summary>
/// Estados posibles de una reserva.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<EstadoReserva>))]
public enum EstadoReserva
{
    Pendiente,
    Confirmada,
    Cancelada
}
