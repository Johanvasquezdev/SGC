using SGC.Domain.Base;
using SGC.Domain.Enums;

namespace SGC.Domain.Entities.Security
{
    // La clase Usuario representa a un usuario del sistema, con propiedades para nombre, email, contraseña (almacenada como hash), rol y estado de activación. Incluye reglas de negocio para activar o desactivar el usuario, asegurando que no se realicen acciones redundantes.
    public abstract class Usuario : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; } = true;

        // regla de negocio
        public void Desactivar()
        {
            if (!Activo)
                throw new InvalidOperationException("El usuario ya está desactivado.");
            Activo = false;
        }

        public void Activar()
        {
            if (Activo)
                throw new InvalidOperationException("El usuario ya está activo.");
            Activo = true;
        }
    }
}