using FluentValidation;
using DoctorApp.DTOs.Requests;

namespace DoctorApp.Validators;

/// <summary>
/// Validador para confirmar cita
/// </summary>
public class ConfirmarCitaValidator : AbstractValidator<ConfirmarCitaRequest>
{
    public ConfirmarCitaValidator()
    {
        RuleFor(x => x.CitaId)
            .GreaterThan(0)
            .WithMessage("El ID de la cita es requerido y debe ser válido");

        RuleFor(x => x.Notas)
            .MaximumLength(500)
            .WithMessage("Las notas no pueden exceder 500 caracteres");
    }
}

/// <summary>
/// Validador para iniciar consulta
/// </summary>
public class IniciarConsultaValidator : AbstractValidator<IniciarConsultaRequest>
{
    public IniciarConsultaValidator()
    {
        RuleFor(x => x.CitaId)
            .GreaterThan(0)
            .WithMessage("El ID de la cita es requerido");
    }
}

/// <summary>
/// Validador para marcar asistencia
/// </summary>
public class MarcarAsistenciaValidator : AbstractValidator<MarcarAsistenciaRequest>
{
    public MarcarAsistenciaValidator()
    {
        RuleFor(x => x.CitaId)
            .GreaterThan(0)
            .WithMessage("El ID de la cita es requerido");

        RuleFor(x => x.Observaciones)
            .MaximumLength(1000)
            .WithMessage("Las observaciones no pueden exceder 1000 caracteres");
    }
}

/// <summary>
/// Validador para crear disponibilidad
/// </summary>
public class CrearDisponibilidadValidator : AbstractValidator<CrearDisponibilidadRequest>
{
    public CrearDisponibilidadValidator()
    {
        RuleFor(x => x.MedicoId)
            .GreaterThan(0)
            .WithMessage("El ID del médico es requerido");

        RuleFor(x => x.DiaSemana)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(7)
            .WithMessage("El día de la semana debe estar entre 1 (Lunes) y 7 (Domingo)");

        RuleFor(x => x.HoraInicio)
            .LessThan(x => x.HoraFin)
            .WithMessage("La hora de inicio debe ser menor a la hora de fin");

        RuleFor(x => x.DuracionMinutos)
            .GreaterThan(0)
            .LessThanOrEqualTo(480) // Máximo 8 horas
            .WithMessage("La duración debe estar entre 1 y 480 minutos");
    }
}

/// <summary>
/// Validador para actualizar disponibilidad
/// </summary>
public class ActualizarDisponibilidadValidator : AbstractValidator<ActualizarDisponibilidadRequest>
{
    public ActualizarDisponibilidadValidator()
    {
        RuleFor(x => x.DisponibilidadId)
            .GreaterThan(0)
            .WithMessage("El ID de disponibilidad es requerido");

        When(x => x.HoraInicio.HasValue && x.HoraFin.HasValue, () =>
        {
            RuleFor(x => x.HoraInicio)
                .Must((x, hora) => hora < x.HoraFin)
                .WithMessage("La hora de inicio debe ser menor a la hora de fin");
        });

        RuleFor(x => x.DuracionMinutos)
            .GreaterThan(0)
            .LessThanOrEqualTo(480)
            .When(x => x.DuracionMinutos.HasValue)
            .WithMessage("La duración debe estar entre 1 y 480 minutos");
    }
}
