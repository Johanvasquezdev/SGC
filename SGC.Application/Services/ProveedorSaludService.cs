using SGC.Application.Contracts;
using SGC.Application.DTOs.Catalog;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class ProveedorSaludService : BaseService, IProveedorSaludService
    {
        private readonly IProveedorSaludRepository _proveedorRepository;
        private readonly ProveedorSaludValidator _validator;

        public ProveedorSaludService(
            IProveedorSaludRepository proveedorRepository,
            ProveedorSaludValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _proveedorRepository = proveedorRepository;
            _validator = validator;
        }

        public async Task<ProveedorSaludResponse> CrearAsync(
            ProveedorSaludRequest request)
        {
            LogOperacion("CrearProveedor", $"Nombre: {request.Nombre}");
            var proveedor = ProveedorSaludMapper.ToEntity(request);
            _validator.Validar(proveedor);
            await _proveedorRepository.AddAsync(proveedor);
            return ProveedorSaludMapper.ToResponse(proveedor);
        }

        public async Task<ProveedorSaludResponse> GetByIdAsync(int id)
        {
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            return ProveedorSaludMapper.ToResponse(proveedor);
        }

        public async Task<IEnumerable<ProveedorSaludResponse>> GetAllAsync()
        {
            var proveedores = await _proveedorRepository.GetAllAsync();
            return proveedores.Select(ProveedorSaludMapper.ToResponse);
        }

        public async Task<IEnumerable<ProveedorSaludResponse>> GetActivosAsync()
        {
            var proveedores = await _proveedorRepository.GetActivosAsync();
            return proveedores.Select(ProveedorSaludMapper.ToResponse);
        }

        public async Task ActualizarAsync(int id, ProveedorSaludRequest request)
        {
            LogOperacion("ActualizarProveedor", $"Id: {id}");
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            proveedor.Nombre = request.Nombre;
            proveedor.Tipo = request.Tipo;
            proveedor.Telefono = request.Telefono;
            proveedor.Email = request.Email;
            _validator.Validar(proveedor);
            await _proveedorRepository.UpdateAsync(proveedor);
        }

        public async Task EliminarAsync(int id)
        {
            LogAdvertencia("EliminarProveedor", $"Id: {id}");
            var proveedor = await _proveedorRepository.GetByIdAsync(id);
            proveedor.Activo = false;
            await _proveedorRepository.UpdateAsync(proveedor);
        }
    }
}