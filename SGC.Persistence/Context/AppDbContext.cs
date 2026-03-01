using Microsoft.EntityFrameworkCore;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Entities.Security;

namespace SGC.Persistence.Context
{
    // DbContext principal de la aplicación.
    // Centraliza el acceso a todas las tablas de la base de datos PostgreSQL (Supabase).
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Módulo de Seguridad
        public DbSet<Usuario> Usuarios { get; set; }

        // Módulo Médico
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        // Módulo de Citas
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }

        // Módulo de Notificaciones
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<PrefNotificacion> PrefsNotificacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de herencia TPT (Table-Per-Type) para el módulo médico.
            // Medico y Paciente heredan de Usuario; cada uno tiene su propia tabla con FK a Usuarios.
            modelBuilder.Entity<Medico>().ToTable("Medicos");
            modelBuilder.Entity<Paciente>().ToTable("Pacientes");
        }
    }
}
