namespace SGC.Domain.Exceptions
{
    public sealed class CitaNotFoundException : Exception // para indicar que no se encontró una cita con el Id especificado
    {
        public CitaNotFoundException(int id)
            : base($"La cita con Id {id} no fue encontrada.") { }
    }
}