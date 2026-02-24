using SGC.Domain.Base;

namespace SGC.Domain.Entities.Catalog
{

    // el proveedor de salud representa a las entidades que suministran servicios médicos, como hospitales, clínicas o laboratorios. Incluye propiedades para el nombre del proveedor, tipo (por ejemplo, hospital, clínica), información de contacto (teléfono y email) y un indicador de si el proveedor está activo. Esta clase es sellada para evitar herencias no deseadas, ya que representa un concepto específico dentro del dominio de la gestión de citas médicas.
    public sealed class ProveedorSalud : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Tipo { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; } = true;
    }
}