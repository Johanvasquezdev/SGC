using Microsoft.EntityFrameworkCore;
using SGC.Domain.Base;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Entities.Payments;
using SGC.Domain.Entities.Security;

namespace SGC.Persistence.Context
{
    public class SGCDbContext : DbContext
    {
        public SGCDbContext(DbContextOptions<SGCDbContext> options)
            : base(options) { }

        // Security
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        // Medical
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        // Catalog
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<ProveedorSalud> ProveedoresSalud { get; set; }

        // Appointments
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }

        // Notifications
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<PrefNotificacion> PrefNotificaciones { get; set; }

        // Audit
        public DbSet<AuditEntity> EventosAuditoria { get; set; }

        // Payments
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPT: Usuario -> Medico, Paciente, Administrador
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<Medico>().ToTable("MEDICO");
            modelBuilder.Entity<Paciente>().ToTable("PACIENTE");
            modelBuilder.Entity<Administrador>().ToTable("ADMINISTRADOR");

            // USUARIO
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("passwordHash").IsRequired();
                entity.Property(e => e.Rol).HasColumnName("rol").HasConversion<string>().IsRequired();
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion").HasDefaultValueSql("now()");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // MEDICO
            modelBuilder.Entity<Medico>(entity =>
            {
                entity.Property(e => e.EspecialidadId).HasColumnName("especialidadId");
                entity.Property(e => e.ProveedorSaludId).HasColumnName("proveedorSaludId");
                entity.Property(e => e.Exequatur).HasColumnName("exequatur");
                entity.Property(e => e.TelefonoConsultorio).HasColumnName("telefonoConsultorio");
                entity.Property(e => e.Foto).HasColumnName("foto");
                entity.Property(e => e.MedicoActivo).HasColumnName("activo").HasDefaultValue(true);
                entity.HasIndex(e => e.EspecialidadId);
                entity.HasIndex(e => e.ProveedorSaludId);
                entity.HasOne(m => m.Especialidad).WithMany(e => e.Medicos).HasForeignKey(m => m.EspecialidadId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(m => m.ProveedorSalud).WithMany(p => p.Medicos).HasForeignKey(m => m.ProveedorSaludId).OnDelete(DeleteBehavior.SetNull);
            });

            // PACIENTE
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.Property(e => e.Cedula).HasColumnName("cedula");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento").HasColumnType("date");
                entity.Property(e => e.TipoSeguro).HasColumnName("tipoSeguro");
                entity.Property(e => e.NumeroSeguro).HasColumnName("numeroSeguro");
            });

            // ESPECIALIDAD
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.ToTable("ESPECIALIDAD");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
            });

            // PROVEEDOR_SALUD
            modelBuilder.Entity<ProveedorSalud>(entity =>
            {
                entity.ToTable("PROVEEDOR_SALUD");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Tipo).HasColumnName("tipo");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
            });

            // DISPONIBILIDAD
            modelBuilder.Entity<Disponibilidad>(entity =>
            {
                entity.ToTable("DISPONIBILIDAD");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MedicoId).HasColumnName("medicoId").IsRequired();
                entity.Property(e => e.DiaSemana).HasColumnName("diaSemana").HasConversion<string>().IsRequired();
                entity.Property(e => e.HoraInicio).HasColumnName("horaInicio").HasColumnType("time").IsRequired();
                entity.Property(e => e.HoraFin).HasColumnName("horaFin").HasColumnType("time").IsRequired();
                entity.Property(e => e.DuracionCitaMin).HasColumnName("duracionCitaMin").IsRequired();
                entity.Property(e => e.EsRecurrente).HasColumnName("esRecurrente").HasDefaultValue(true);
                entity.HasIndex(e => e.MedicoId);
                entity.HasOne(d => d.Medico).WithMany(m => m.Horarios).HasForeignKey(d => d.MedicoId).OnDelete(DeleteBehavior.Cascade);
            });

            // CITA
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("CITA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PacienteId).HasColumnName("pacienteId").IsRequired();
                entity.Property(e => e.MedicoId).HasColumnName("medicoId").IsRequired();
                entity.Property(e => e.DisponibilidadId).HasColumnName("disponibilidadId");
                entity.Property(e => e.FechaHora).HasColumnName("fechaHora").IsRequired();
                entity.Property(e => e.Estado).HasColumnName("estado").HasConversion<string>().IsRequired();
                entity.Property(e => e.Motivo).HasColumnName("motivo");
                entity.Property(e => e.Notas).HasColumnName("notas");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion").HasDefaultValueSql("now()");
                entity.HasIndex(e => e.PacienteId);
                entity.HasIndex(e => e.MedicoId);
                entity.HasIndex(e => e.DisponibilidadId);
                entity.HasOne(c => c.Paciente).WithMany(p => p.Citas).HasForeignKey(c => c.PacienteId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Medico).WithMany(m => m.Citas).HasForeignKey(c => c.MedicoId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Disponibilidad).WithMany().HasForeignKey(c => c.DisponibilidadId).OnDelete(DeleteBehavior.SetNull);
            });

            // EVENTO_AUDITORIA
            modelBuilder.Entity<AuditEntity>(entity =>
            {
                entity.ToTable("EVENTO_AUDITORIA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
                entity.Property(e => e.Entidad).HasColumnName("entidad").IsRequired();
                entity.Property(e => e.Accion).HasColumnName("accion").IsRequired();
                entity.Property(e => e.ValorAnterior).HasColumnName("valorAnterior");
                entity.Property(e => e.ValorNuevo).HasColumnName("valorNuevo");
                entity.Property(e => e.Fecha).HasColumnName("fecha").HasDefaultValueSql("now()");
                entity.Property(e => e.DireccionIP).HasColumnName("direccionIP");
                entity.HasIndex(e => e.UsuarioId);
                entity.HasOne(a => a.Usuario).WithMany().HasForeignKey(a => a.UsuarioId).OnDelete(DeleteBehavior.SetNull);
            });

            // NOTIFICACION
            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.ToTable("NOTIFICACION");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId").IsRequired();
                entity.Property(e => e.CitaId).HasColumnName("citaId");
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasConversion<string>().IsRequired();
                entity.Property(e => e.Mensaje).HasColumnName("mensaje").IsRequired();
                entity.Property(e => e.Leida).HasColumnName("leida").HasDefaultValue(false);
                entity.Property(e => e.FechaEnvio).HasColumnName("fechaEnvio").HasDefaultValueSql("now()");
                entity.HasIndex(e => e.UsuarioId);
                entity.HasIndex(e => e.CitaId);
                entity.HasOne(n => n.Usuario).WithMany(u => u.Notificaciones).HasForeignKey(n => n.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(n => n.Cita).WithMany().HasForeignKey(n => n.CitaId).OnDelete(DeleteBehavior.Cascade);
            });

            // PREF_NOTIFICACION
            modelBuilder.Entity<PrefNotificacion>(entity =>
            {
                entity.ToTable("PREF_NOTIFICACION");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId").IsRequired();
                entity.Property(e => e.RecibirEmail).HasColumnName("recibirEmail").HasDefaultValue(true);
                entity.Property(e => e.RecibirSMS).HasColumnName("recibirSMS").HasDefaultValue(true);
                entity.Property(e => e.RecibirPush).HasColumnName("recibirPush").HasDefaultValue(true);
                entity.Property(e => e.HorasAntesRecordatorio).HasColumnName("horasAntesRecordatorio").HasDefaultValue(24);
                entity.HasIndex(e => e.UsuarioId).IsUnique();
                entity.HasOne(p => p.Usuario).WithOne(u => u.PrefNotificacion).HasForeignKey<PrefNotificacion>(p => p.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            });

            // PAGO
            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("PAGO");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CitaId).HasColumnName("citaId").IsRequired();
                entity.Property(e => e.PacienteId).HasColumnName("pacienteId").IsRequired();
                entity.Property(e => e.Monto).HasColumnName("monto").IsRequired();
                entity.Property(e => e.Moneda).HasColumnName("moneda").HasDefaultValue("DOP");
                entity.Property(e => e.Estado).HasColumnName("estado").HasConversion<string>().IsRequired();
                entity.Property(e => e.StripePaymentIntentId).HasColumnName("stripePaymentIntentId");
                entity.Property(e => e.FechaPago).HasColumnName("fechaPago");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion").HasDefaultValueSql("now()");
                entity.HasIndex(e => e.CitaId);
                entity.HasIndex(e => e.PacienteId);
                entity.HasOne(p => p.Cita).WithMany().HasForeignKey(p => p.CitaId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.Paciente).WithMany().HasForeignKey(p => p.PacienteId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}