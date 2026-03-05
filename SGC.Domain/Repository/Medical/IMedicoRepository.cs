using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Repository.Medical
{
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        Task<Medico> GetByExequaturAsync(string exequatur);
        Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId);
    }
}
