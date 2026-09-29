using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos;

/// <summary>
/// Datos necesarios para registrar una reserva.
/// </summary>
public class CreateReservaRequestDto : IValidatableObject
{
    /// <summary>
    /// Nombre de la sala a reservar.
    /// </summary>
    [Required(ErrorMessage = "El nombre de la sala es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre de la sala debe tener entre 2 y 100 caracteres.")]
    public string NombreSala { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de la reserva (yyyy-MM-dd).
    /// </summary>
    [Required(ErrorMessage = "La fecha de reserva es obligatoria.")]
    public DateOnly? FechaReserva { get; set; }

    /// <summary>
    /// Hora de inicio (HH:mm:ss).
    /// </summary>
    [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
    public TimeOnly? HoraInicio { get; set; }

    /// <summary>
    /// Hora de fin (HH:mm:ss). Debe ser posterior a la hora de inicio.
    /// </summary>
    [Required(ErrorMessage = "La hora de fin es obligatoria.")]
    public TimeOnly? HoraFin { get; set; }

    /// <summary>
    /// Nombre del responsable de la reserva.
    /// </summary>
    [Required(ErrorMessage = "El nombre del responsable es obligatorio.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre del responsable debe tener entre 2 y 150 caracteres.")]
    public string NombreResponsable { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de asistentes. Debe ser mayor a cero.
    /// </summary>
    [Range(1, 1000, ErrorMessage = "La cantidad de asistentes debe ser mayor a cero y menor o igual a 1000.")]
    public int CantidadAsistentes { get; set; }

    /// <summary>
    /// Motivo de la reunión.
    /// </summary>
    [Required(ErrorMessage = "El motivo de la reunión es obligatorio.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 300 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HoraInicio.HasValue && HoraFin.HasValue && HoraFin <= HoraInicio)
            yield return new ValidationResult("La hora de fin debe ser posterior a la hora de inicio.", [nameof(HoraFin)]);
    }
}
