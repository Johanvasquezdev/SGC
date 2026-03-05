namespace SGC.Application.DTOs.Appointments
{
    public class DisponibilidadDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string DiaSemana { get; set; } = null!;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
