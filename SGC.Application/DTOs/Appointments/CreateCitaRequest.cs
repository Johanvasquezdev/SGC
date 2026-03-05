namespace SGC.Application.DTOs.Appointments
{
    public class CreateCitaRequest
    {
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = null!;
    }
}
