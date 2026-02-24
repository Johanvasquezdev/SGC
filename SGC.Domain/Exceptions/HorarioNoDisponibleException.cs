namespace SGC.Domain.Exceptions
{
    public sealed class HorarioNoDisponibleException : Exception // para indicar que el médico no tiene disponibilidad para la fecha y hora solicitada
    {
        public HorarioNoDisponibleException(int medicoId, DateTime fechaHora)
            : base($"El médico {medicoId} no tiene disponibilidad para {fechaHora}.") { }
    }
}