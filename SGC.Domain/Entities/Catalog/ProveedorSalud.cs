using SGC.Domain.Base;

namespace SGC.Domain.Entities.Catalog
{
    public sealed class ProveedorSalud : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Tipo { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; } = true;
    }
}