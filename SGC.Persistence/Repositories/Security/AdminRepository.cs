using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Security;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace SGC.Persistence.Repositories.Security
{
    // Repositorio específico para la entidad Administrador, implementa métodos adicionales si es necesario
    public class AdministradorRepository
        : BaseRepository<Administrador>, IAdministradorRepository
    {
        // Constructor que recibe el contexto de la base de datos y lo pasa a la clase base
        public AdministradorRepository(SGCDbContext context)
            : base(context) { }

        // Obtiene un administrador por su email (para autenticacion)
        public async Task<Administrador?> GetByEmailAsync(string email)
        {
            return await Context.Administradores
                .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}