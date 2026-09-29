namespace DemoApi.Models;

/// <summary>
/// Representa la reserva de una sala de reuniones.
/// </summary>
public class Reserva
{
    /// <summary>
    /// Identificador único de la reserva.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de la sala reservada.
    /// </summary>
    public string NombreSala { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de la reserva.
    /// </summary>
    public DateOnly FechaReserva { get; set; }

    /// <summary>
    /// Hora de inicio de la reunión.
    /// </summary>
    public TimeOnly HoraInicio { get; set; }

    /// <summary>
    /// Hora de fin de la reunión.
    /// </summary>
    public TimeOnly HoraFin { get; set; }

    /// <summary>
    /// Nombre de la persona responsable de la reserva.
    /// </summary>
    public string NombreResponsable { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de asistentes a la reunión.
    /// </summary>
    public int CantidadAsistentes { get; set; }

    /// <summary>
    /// Motivo de la reunión.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Estado actual de la reserva.
    /// </summary>
    public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
}
