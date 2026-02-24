using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        Task<Paciente> GetByCedulaAsync(string cedula);
    }
}
