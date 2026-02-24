using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Interfaces.Repository
{
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        Task<Medico> GetByExequaturAsync(string exequatur);
        Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId);
    }
}
