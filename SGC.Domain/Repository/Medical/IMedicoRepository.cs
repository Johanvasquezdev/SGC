using SGC.Domain.Entities.Medical;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Domain.Repository.Medical
{
    // Interfaz de repositorio específica para la entidad Medico.
    // Extiende las operaciones CRUD base con consultas propias del contexto médico.
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        // Recupera todos los médicos que pertenecen a una especialidad determinada.
        Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId);
    }
}
