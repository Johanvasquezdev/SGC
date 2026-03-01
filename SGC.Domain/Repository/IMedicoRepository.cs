using System.Collections.Generic;
using System.Threading.Tasks;
using SGC.Domain.Entities.Medical;

namespace SGC.Domain.Repository
{
    // Repositorio específico para la entidad Medico.
    // Extiende las operaciones CRUD base con consultas propias del módulo médico.
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        // Obtiene todos los médicos que pertenecen a una especialidad determinada.
        // Útil para el flujo de búsqueda de médicos por parte del paciente.
        Task<IEnumerable<Medico>> GetByEspecialidadAsync(int especialidadId);
    }
}
