namespace SGC.Domain.Exceptions
{
    public sealed class CitaConflictoException : Exception // para indicar que hay un conflicto de citas (ej: un médico ya tiene una cita programada en ese horario) 
    {
        public CitaConflictoException(string mensaje)
            : base(mensaje) { }
    }
}