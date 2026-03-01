using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IPacienteRepository : IBaseRepository<Paciente> // Interfaz específica para la entidad Paciente, que hereda de la interfaz genérica IBaseRepository. Esto permite mantener las operaciones CRUD estándar mientras se agregan métodos específicos para la gestión de pacientes.
    {
        Task<Paciente> GetByCedulaAsync(string cedula); // Método específico para obtener un paciente por su número de cédula, que es un identificador único en muchos países. Esto es útil para evitar duplicados y facilitar la búsqueda de pacientes.
    }
}
