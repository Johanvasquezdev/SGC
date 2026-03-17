using Microsoft.EntityFrameworkCore;
using SGC.Domain.Base;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace SGC.Persistence.Repositories.Audit
{
    // Repositorio específico para la entidad de auditoría, implementando métodos personalizados para consultas
    public class AuditoriaRepository : BaseRepository<AuditEntity>, IAuditoriaRepository
    {
        public AuditoriaRepository(SGCDbContext context) : base(context) { }

        // Métodos personalizados para obtener eventos de auditoría por diferentes criterios
        public async Task<IEnumerable<AuditEntity>> GetByEntidadAsync(string entidad)
        {
            return await Context.EventosAuditoria
                .Where(a => a.Entidad == entidad)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditEntity>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await Context.EventosAuditoria
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditEntity>> GetByFechaAsync(DateTime fecha)
        {
            return await Context.EventosAuditoria
                .Where(a => a.Fecha.Date == fecha.Date)
                .OrderByDescending(a => a.Fecha)
                .ToListAsync();
        }
    }
}