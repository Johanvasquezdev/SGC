using System.Threading.Tasks;
using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad Paciente.
    // Extiende las operaciones CRUD base con consultas propias del módulo médico.
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        // Busca un paciente por su número de cédula de identidad.
        // Permite identificar de forma única a un paciente en el sistema.
        Task<Paciente?> GetByCedulaAsync(string cedula);
    }
}
