using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Payments;
using SGC.Domain.Enums;
using SGC.Domain.Interfaces.Repository;
using SGC.Persistence.Base;
using SGC.Persistence.Context;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace SGC.Persistence.Repositories.Payments
{
    
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(SGCDbContext context) : base(context) { }

        // Permite obtener un pago específico asociado a una cita médica, utilizando el identificador de la cita como criterio de búsqueda
        public async Task<Pago?> GetByCitaAsync(int citaId)
        {
            return await Context.Pagos
                .FirstOrDefaultAsync(p => p.CitaId == citaId);
        }

        // 
        public async Task<Pago?> GetByStripeIdAsync(string stripePaymentIntentId)
        {
            return await Context.Pagos
                .FirstOrDefaultAsync(p =>
                    p.StripePaymentIntentId == stripePaymentIntentId);
        }

        // Permite obtener todos los pagos asociados a un paciente específico, ordenados por fecha de creación (más recientes primero)
        public async Task<IEnumerable<Pago>> GetByPacienteAsync(int pacienteId)
        {
            return await Context.Pagos
                .Where(p => p.PacienteId == pacienteId)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        // Permite obtener todos los pagos filtrados por su estado (Pendiente, Completado, Fallido, Reembolsado)
        public async Task<IEnumerable<Pago>> GetByEstadoAsync(EstadoPago estado)
        {
            return await Context.Pagos
                .Where(p => p.Estado == estado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }
    }
}