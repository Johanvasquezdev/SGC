using SGC.Application.DTOs.Catalog;
using SGC.Domain.Entities.Catalog;

namespace SGC.Application.Mappers
{
    public static class ProveedorSaludMapper
    {
        public static ProveedorSaludResponse ToResponse(
            ProveedorSalud proveedor)
        {
            return new ProveedorSaludResponse
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Tipo = proveedor.Tipo,
                Telefono = proveedor.Telefono,
                Email = proveedor.Email,
                Activo = proveedor.Activo
            };
        }

        public static ProveedorSalud ToEntity(
            ProveedorSaludRequest request)
        {
            return new ProveedorSalud
            {
                Nombre = request.Nombre,
                Tipo = request.Tipo,
                Telefono = request.Telefono,
                Email = request.Email
            };
        }
    }
}