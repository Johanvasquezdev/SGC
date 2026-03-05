using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Entities.Security;

namespace SGC.Persistence.Context
{
    public class SGCDbContext : DbContext
    {
        public SGCDbContext(DbContextOptions<SGCDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<PrefNotificacion> PrefNotificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Nombre).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Rol)
                    .HasConversion<string>();
            });

            modelBuilder.Entity<Medico>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Exequatur).IsRequired();
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Cedula).IsRequired();
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<Disponibilidad>(entity =>
            {
                entity.HasKey(d => d.Id);
            });

            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.HasKey(n => n.Id);
            });

            modelBuilder.Entity<PrefNotificacion>(entity =>
            {
                entity.HasKey(p => p.Id);
            });
        }
    }
}
