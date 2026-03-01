using Microsoft.EntityFrameworkCore;
using SGC.Domain.Base;
using SGC.Domain.Entities.Appointments;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Entities.Medical;
using SGC.Domain.Entities.Notifications;
using SGC.Domain.Entities.Security;

namespace SGC.Persistence.Context
{
    #region DBContext
    public class SGCDbContext : DbContext
    {
        public SGCDbContext(DbContextOptions<SGCDbContext> options) : base(options) { }

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

        #endregion
        #region ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================================
            // TPT Inheritance: Usuario -> Medico, Paciente, Administrador
            // Each subtype maps to its own table with PK = FK -> USUARIO.id
            // =============================================
            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<Medico>().ToTable("MEDICO");
            modelBuilder.Entity<Paciente>().ToTable("PACIENTE");
            modelBuilder.Entity<Administrador>().ToTable("ADMINISTRADOR");

            // =============================================
            // USUARIO
            // =============================================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("passwordHash").IsRequired();
                entity.Property(e => e.Rol)
                      .HasColumnName("rol")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(e => e.FechaCreacion)
                      .HasColumnName("fechaCreacion")
                      .HasDefaultValueSql("now()");
                entity.Property(e => e.Activo)
                      .HasColumnName("activo")
                      .HasDefaultValue(true);

                entity.HasIndex(e => e.Email).IsUnique();
            });

            // =============================================
            // MEDICO
            // =============================================
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
            });

            // =============================================
            // PACIENTE
            // =============================================
            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.Property(e => e.Cedula).HasColumnName("cedula");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.FechaNacimiento).HasColumnName("fechaNacimiento");
                entity.Property(e => e.TipoSeguro).HasColumnName("tipoSeguro");
                entity.Property(e => e.NumeroSeguro).HasColumnName("numeroSeguro");
            });

            // =============================================
            // ESPECIALIDAD
            // =============================================
            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.ToTable("ESPECIALIDAD");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired();
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
            });

            // =============================================
            // PROVEEDOR_SALUD
            // =============================================
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

            // =============================================
            // DISPONIBILIDAD
            // =============================================
            modelBuilder.Entity<Disponibilidad>(entity =>
            {
                entity.ToTable("DISPONIBILIDAD");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.MedicoId).HasColumnName("medicoId").IsRequired();
                entity.Property(e => e.DiaSemana)
                      .HasColumnName("diaSemana")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(e => e.HoraInicio).HasColumnName("horaInicio").IsRequired();
                entity.Property(e => e.HoraFin).HasColumnName("horaFin").IsRequired();
                entity.Property(e => e.DuracionCitaMin).HasColumnName("duracionCitaMin").IsRequired();
                entity.Property(e => e.EsRecurrente)
                      .HasColumnName("esRecurrente")
                      .HasDefaultValue(true);

                entity.HasIndex(e => e.MedicoId);
            });

            // =============================================
            // CITA
            // =============================================
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("CITA");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PacienteId).HasColumnName("pacienteId").IsRequired();
                entity.Property(e => e.MedicoId).HasColumnName("medicoId").IsRequired();
                entity.Property(e => e.DisponibilidadId).HasColumnName("disponibilidadId");
                entity.Property(e => e.FechaHora).HasColumnName("fechaHora").IsRequired();
                entity.Property(e => e.Estado)
                      .HasColumnName("estado")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(e => e.Motivo).HasColumnName("motivo");
                entity.Property(e => e.Notas).HasColumnName("notas");
                entity.Property(e => e.FechaCreacion)
                      .HasColumnName("fechaCreacion")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(e => e.PacienteId);
                entity.HasIndex(e => e.MedicoId);
                entity.HasIndex(e => e.DisponibilidadId);
            });

            // =============================================
            // EVENTO_AUDITORIA
            // =============================================
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
                entity.Property(e => e.Fecha)
                      .HasColumnName("fecha")
                      .HasDefaultValueSql("now()");
                entity.Property(e => e.DireccionIP).HasColumnName("direccionIP");

                entity.HasIndex(e => e.UsuarioId);
            });

            // =============================================
            // NOTIFICACION
            // =============================================
            modelBuilder.Entity<Notificacion>(entity =>
            {
                entity.ToTable("NOTIFICACION");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId").IsRequired();
                entity.Property(e => e.CitaId).HasColumnName("citaId");
                entity.Property(e => e.Tipo)
                      .HasColumnName("tipo")
                      .HasConversion<string>()
                      .IsRequired();
                entity.Property(e => e.Mensaje).HasColumnName("mensaje").IsRequired();
                entity.Property(e => e.Leida)
                      .HasColumnName("leida")
                      .HasDefaultValue(false);
                entity.Property(e => e.FechaEnvio)
                      .HasColumnName("fechaEnvio")
                      .HasDefaultValueSql("now()");

                entity.HasIndex(e => e.UsuarioId);
                entity.HasIndex(e => e.CitaId);
            });

            // =============================================
            // PREF_NOTIFICACION
            // =============================================
            modelBuilder.Entity<PrefNotificacion>(entity =>
            {
                entity.ToTable("PREF_NOTIFICACION");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioId").IsRequired();
                entity.Property(e => e.RecibirEmail)
                      .HasColumnName("recibirEmail")
                      .HasDefaultValue(true);
                entity.Property(e => e.RecibirSMS)
                      .HasColumnName("recibirSMS")
                      .HasDefaultValue(true);
                entity.Property(e => e.RecibirPush)
                      .HasColumnName("recibirPush")
                      .HasDefaultValue(true);
                entity.Property(e => e.HorasAntesRecordatorio)
                      .HasColumnName("horasAntesRecordatorio")
                      .HasDefaultValue(24);

                entity.HasIndex(e => e.UsuarioId).IsUnique();
            });
        }
    }
}

#endregion