using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Security;
using SGC.Domain.Enums;
using SGC.Domain.Exceptions;

namespace SGC.Domain.Entities.Medical
{
    #region Medico
    public sealed class Medico : Usuario // el médico hereda de usuario porque también es un usuario del sistema, pero con atributos y comportamientos específicos relacionados con su rol como médico
    {
        public string? Exequatur { get; set; } // el numero de exequatur es un identificador unico que se le asigna a cada medico para certificar que esta autorizado para ejercer la medicina. Nullable porque el perfil del medico puede crearse antes de tener el exequatur registrado.
        public int? EspecialidadId { get; set; }
        public int? ProveedorSaludId { get; set; }
        public string? TelefonoConsultorio { get; set; }
        public string? Foto { get; set; }
        public bool MedicoActivo { get; set; } = true; // Estado activo del medico como profesional, independiente del estado activo de su cuenta de usuario (Usuario.Activo). La tabla MEDICO tiene su propia columna "activo" separada de USUARIO."activo".
        public List<Disponibilidad> Horarios { get; set; } = new();
        #endregion


        #region Metodos
        // Convierte el DayOfWeek de .NET (en ingles) al enum DiaSemana (en espanol) para que coincida con los valores de la base de datos
        private static DiaSemana ConvertirDiaSemana(DayOfWeek dia) => dia switch
        {
            DayOfWeek.Monday => DiaSemana.Lunes,
            DayOfWeek.Tuesday => DiaSemana.Martes,
            DayOfWeek.Wednesday => DiaSemana.Miercoles,
            DayOfWeek.Thursday => DiaSemana.Jueves,
            DayOfWeek.Friday => DiaSemana.Viernes,
            DayOfWeek.Saturday => DiaSemana.Sabado,
            DayOfWeek.Sunday => DiaSemana.Domingo,
            _ => throw new ArgumentOutOfRangeException(nameof(dia))
        };

        // verifica si el médico tiene disponibilidad
        public bool TieneDisponibilidad(DateTime fechaHora)
        {
            var diaSemana = ConvertirDiaSemana(fechaHora.DayOfWeek);
            return Horarios.Any(h =>
                h.DiaSemana == diaSemana &&
                h.HoraInicio <= fechaHora.TimeOfDay &&
                h.HoraFin >= fechaHora.TimeOfDay &&
                h.EsValido());
        }


        // agrega un horario validando que no se solape
        public void AgregarHorario(Disponibilidad nuevaDisponibilidad)
        {
            if (!nuevaDisponibilidad.EsValido())
                throw new HorarioNoDisponibleException(Id, DateTime.UtcNow);

            bool seSolapa = Horarios.Any(h =>
                h.DiaSemana == nuevaDisponibilidad.DiaSemana &&
                h.HoraInicio < nuevaDisponibilidad.HoraFin &&
                h.HoraFin > nuevaDisponibilidad.HoraInicio);

            if (seSolapa)
                throw new CitaConflictoException("El horario se solapa con uno existente.");

            Horarios.Add(nuevaDisponibilidad);
        }
    }
} 
#endregion