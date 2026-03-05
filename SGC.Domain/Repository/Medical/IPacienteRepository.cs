using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Repository.Medical
{
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        Task<Paciente> GetByCedulaAsync(string cedula);
    }
}
