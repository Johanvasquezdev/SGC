using SGC.Domain.Entities.Medical;
using System.Threading.Tasks;

namespace SGC.Domain.Repository.Medical
{
    // Interfaz de repositorio específica para la entidad Paciente.
    // Extiende las operaciones CRUD base con consultas propias del contexto de pacientes.
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        // Busca un paciente por su número de cédula de identidad.
        Task<Paciente> GetByCedulaAsync(string cedula);
    }
}
