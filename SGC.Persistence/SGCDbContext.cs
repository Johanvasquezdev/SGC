using Microsoft.EntityFrameworkCore;

namespace SGC.Persistence
{
    public class SGCDbContext : DbContext
    {
        public SGCDbContext(DbContextOptions<SGCDbContext> options) : base(options) { }

        // DbSets for each entity
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<PrefNotificacion> PrefNotificaciones { get; set; }
    }
}