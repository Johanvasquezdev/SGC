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
    // Servicio de aplicacion para gestionar las operaciones relacionadas con la entidad Especialidad, implementando la interfaz IEspecialidadService.
    public class EspecialidadService : BaseService, IEspecialidadService
    {
        private readonly IEspecialidadRepository _especialidadRepository;
        private readonly EspecialidadValidator _validator;

        public EspecialidadService(
            IEspecialidadRepository especialidadRepository,
            EspecialidadValidator validator,
            ISGCLogger logger) : base(logger)
        {
            _especialidadRepository = especialidadRepository;
            _validator = validator;
        }

        // Crea una nueva especialidad, validando los datos y registrando la operacion en el log.
        public async Task<EspecialidadResponse> CrearAsync(EspecialidadRequest request)
        {
            LogOperacion("CrearEspecialidad", $"Nombre: {request.Nombre}");
            var especialidad = EspecialidadMapper.ToEntity(request);
            _validator.Validar(especialidad);
            await _especialidadRepository.AddAsync(especialidad);
            return EspecialidadMapper.ToResponse(especialidad);
        }

        // Obtiene una especialidad por su ID, registrando la consulta en el log.
        public async Task<EspecialidadResponse> GetByIdAsync(int id)
        {
            var especialidad = await _especialidadRepository.GetByIdAsync(id);
            return EspecialidadMapper.ToResponse(especialidad);
        }

        // Obtiene todas las especialidades, registrando la consulta en el log.
        public async Task<IEnumerable<EspecialidadResponse>> GetAllAsync()
        {
            var especialidades = await _especialidadRepository.GetAllAsync();
            return especialidades.Select(EspecialidadMapper.ToResponse);
        }

        }
    // Obtiene solo las especialidades activas, registrando la consulta en el log.
        public async Task<IEnumerable<EspecialidadResponse>> GetActivasAsync()
        {
            var especialidades = await _especialidadRepository.GetActivasAsync();
            return especialidades.Select(EspecialidadMapper.ToResponse);
        }

        // Actualiza una especialidad existente, validando los datos y registrando la operacion en el log.
        public async Task ActualizarAsync(int id, EspecialidadRequest request)
        {
            LogOperacion("ActualizarEspecialidad", $"Id: {id}");
            var especialidad = await _especialidadRepository.GetByIdAsync(id);
            especialidad.Nombre = request.Nombre;
            especialidad.Descripcion = request.Descripcion;
            _validator.Validar(especialidad);
            await _especialidadRepository.UpdateAsync(especialidad);
        }

        // Elimina (desactiva) una especialidad, registrando la operacion en el log.
        public async Task EliminarAsync(int id)
        {
            LogAdvertencia("EliminarEspecialidad", $"Id: {id}");
            var especialidad = await _especialidadRepository.GetByIdAsync(id);
            especialidad.Activo = false;
            await _especialidadRepository.UpdateAsync(especialidad);
        }
    }
}