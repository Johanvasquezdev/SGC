namespace SGC.Domain.Exceptions
{
    public sealed class CitaNotFoundException : Exception
    {
        public CitaNotFoundException(int citaId)
            : base($"No se encontró la cita con Id {citaId}.") { }
    }
}
