using SGC.Domain.Entities.Security;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces.Repository
{
    // Interfaz para el repositorio de administradores, que hereda de la interfaz base de repositorios y agrega métodos específicos para la entidad Administrador
    public interface IAdministradorRepository
        : IBaseRepository<Administrador>
    {
        Task<Administrador?> GetByEmailAsync(string email);
    }
}