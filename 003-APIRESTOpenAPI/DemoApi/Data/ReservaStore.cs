using DemoApi.Models;

namespace DemoApi.Data;

/// <summary>
/// Almacenamiento en memoria de las reservas. Se registra como singleton.
/// </summary>
public class ReservaStore
{
    private readonly List<Reserva> _reservas = new();
    private readonly Lock _lock = new();
    private int _nextId = 1;

    public ReservaStore()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        Add(new Reserva { NombreSala = "Sala Andes", FechaReserva = hoy, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(10, 0), NombreResponsable = "Ana Torres", CantidadAsistentes = 6, Motivo = "Reunión de planificación semanal", Estado = EstadoReserva.Confirmada });
        Add(new Reserva { NombreSala = "Sala Pacífico", FechaReserva = hoy, HoraInicio = new TimeOnly(11, 0), HoraFin = new TimeOnly(12, 30), NombreResponsable = "Luis Ramírez", CantidadAsistentes = 10, Motivo = "Revisión de proyecto", Estado = EstadoReserva.Pendiente });
        Add(new Reserva { NombreSala = "Sala Andes", FechaReserva = hoy.AddDays(1), HoraInicio = new TimeOnly(15, 0), HoraFin = new TimeOnly(16, 0), NombreResponsable = "María Quispe", CantidadAsistentes = 4, Motivo = "Entrevista de candidatos", Estado = EstadoReserva.Pendiente });
    }

    public List<Reserva> GetAll(DateOnly? fecha = null)
    {
        lock (_lock)
        {
            return _reservas
                .Where(r => fecha is null || r.FechaReserva == fecha)
                .OrderBy(r => r.FechaReserva).ThenBy(r => r.HoraInicio)
                .ToList();
        }
    }

    public Reserva? GetById(int id)
    {
        lock (_lock)
        {
            return _reservas.FirstOrDefault(r => r.Id == id);
        }
    }

    /// <summary>
    /// Indica si la sala ya tiene una reserva activa (no cancelada) que se cruza con el horario indicado.
    /// </summary>
    public bool HasOverlap(string nombreSala, DateOnly fecha, TimeOnly inicio, TimeOnly fin, int? excludeId = null)
    {
        lock (_lock)
        {
            return _reservas.Any(r =>
                r.Id != excludeId &&
                r.Estado != EstadoReserva.Cancelada &&
                r.FechaReserva == fecha &&
                string.Equals(r.NombreSala, nombreSala, StringComparison.OrdinalIgnoreCase) &&
                inicio < r.HoraFin && r.HoraInicio < fin);
        }
    }

    public Reserva Add(Reserva reserva)
    {
        lock (_lock)
        {
            reserva.Id = _nextId++;
            _reservas.Add(reserva);
            return reserva;
        }
    }

    public bool Remove(int id)
    {
        lock (_lock)
        {
            return _reservas.RemoveAll(r => r.Id == id) > 0;
        }
    }
}
