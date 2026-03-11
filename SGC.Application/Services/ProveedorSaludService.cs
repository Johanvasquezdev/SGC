using SGC.Application.Contracts;
using SGC.Application.DTOs.Catalog;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de proveedores de salud
    public class ProveedorSaludService : IProveedorSaludService
    {
        // Repositorio de proveedores para acceso a datos
        private readonly IProveedorSaludRepository _proveedorRepository;

        // Validador de reglas de negocio para proveedores de salud
        private readonly ProveedorSaludValidator _validator = new ProveedorSaludValidator();

        public ProveedorSaludService(IProveedorSaludRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        // Crea un nuevo proveedor de salud en el sistema
        public async Task<ProveedorSaludResponse> CrearAsync(ProveedorSaludRequest request)
        {
            var proveedor = new ProveedorSalud
            {
                Nombre = request.Nombre,
                Tipo = request.Tipo,
                Telefono = request.Telefono,
                Email = request.Email
            };

            // Validar reglas de negocio antes de guardar
            _validator.Validar(proveedor);

            await _proveedorRepository.AddAsync(proveedor);
            return MapToResponse(proveedor);
        }

        // Obtiene un proveedor por su identificador
        public async Task<ProveedorSaludResponse> GetByIdAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            return MapToResponse(proveedor);
        }

        // Obtiene todos los proveedores del sistema
        public async Task<IEnumerable<ProveedorSaludResponse>> GetAllAsync()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();
            return proveedores.Select(MapToResponse);
        }

        // Obtiene solo los proveedores que estan activos
        public async Task<IEnumerable<ProveedorSaludResponse>> GetActivosAsync()
        {
            var proveedores = await _proveedorRepository.GetActivosAsync();
            return proveedores.Select(MapToResponse);
        }

        // Actualiza la informacion de un proveedor existente
        public async Task ActualizarAsync(int id, ProveedorSaludRequest request)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);

            proveedor.Nombre = request.Nombre;
            proveedor.Tipo = request.Tipo;
            proveedor.Telefono = request.Telefono;
            proveedor.Email = request.Email;

            // Validar reglas de negocio antes de actualizar
            _validator.Validar(proveedor);

            await _proveedorRepository.UpdateAsync(proveedor);
        }

        // Desactiva un proveedor de salud (borrado logico)
        public async Task EliminarAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            proveedor.Activo = false;
            await _proveedorRepository.UpdateAsync(proveedor);
        }

        // Convierte una entidad ProveedorSalud a su DTO de respuesta
        private static ProveedorSaludResponse MapToResponse(ProveedorSalud p)
        {
            return new ProveedorSaludResponse
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Tipo = p.Tipo,
                Telefono = p.Telefono,
                Email = p.Email,
                Activo = p.Activo
            };
        }
    }
}
