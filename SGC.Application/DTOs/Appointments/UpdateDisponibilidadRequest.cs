namespace SGC.Application.DTOs.Appointments
{
    public class UpdateDisponibilidadRequest
    {
        public string DiaSemana { get; set; } = null!;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
