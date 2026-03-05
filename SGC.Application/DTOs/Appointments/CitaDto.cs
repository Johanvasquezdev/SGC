namespace SGC.Application.DTOs.Appointments
{
    public class CitaDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
        public string Motivo { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
