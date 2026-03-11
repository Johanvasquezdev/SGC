using SGC.Application.Contracts;
using SGC.Application.DTOs.Catalog;
using SGC.Domain.Entities.Catalog;
using SGC.Domain.Interfaces.Repository;
using SGC.Domain.Validators;

namespace SGC.Application.Services
{
    // Servicio de aplicacion para la gestion de especialidades medicas
    public class EspecialidadService : IEspecialidadService
    {
        // Repositorio de especialidades para acceso a datos
        private readonly IEspecialidadRepository _especialidadRepository;

        // Validador de reglas de negocio para especialidades
        private readonly EspecialidadValidator _validator = new EspecialidadValidator();

        public EspecialidadService(IEspecialidadRepository especialidadRepository)
        {
            _especialidadRepository = especialidadRepository;
        }

        // Crea una nueva especialidad medica en el sistema
        public async Task<EspecialidadResponse> CrearAsync(EspecialidadRequest request)
        {
            var especialidad = new Especialidad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            };

            // Validar reglas de negocio antes de guardar
            _validator.Validar(especialidad);

            await _especialidadRepository.AddAsync(especialidad);
            return MapToResponse(especialidad);
        }

        // Obtiene una especialidad por su identificador
        public async Task<EspecialidadResponse> GetByIdAsync(int id)
        {
            var especialidad = await _especialidadRepository.GetByIdAsync(id);
            return MapToResponse(especialidad);
        }

        // Obtiene todas las especialidades del sistema
        public async Task<IEnumerable<EspecialidadResponse>> GetAllAsync()
        {
            var especialidades = await _especialidadRepository.GetAllAsync();
            return especialidades.Select(MapToResponse);
        }

        // Obtiene solo las especialidades que estan activas
        public async Task<IEnumerable<EspecialidadResponse>> GetActivasAsync()
        {
            var especialidades = await _especialidadRepository.GetActivasAsync();
            return especialidades.Select(MapToResponse);
        }

        // Actualiza el nombre y descripcion de una especialidad existente
        public async Task ActualizarAsync(int id, EspecialidadRequest request)
        {
            var especialidad = await _especialidadRepository.GetByIdAsync(id);

            especialidad.Nombre = request.Nombre;
            especialidad.Descripcion = request.Descripcion;

            // Validar reglas de negocio antes de actualizar
            _validator.Validar(especialidad);

            await _especialidadRepository.UpdateAsync(especialidad);
        }

        // Desactiva una especialidad (borrado logico)
        public async Task EliminarAsync(int id)
        {
            var especialidad = await _especialidadRepository.GetByIdAsync(id);
            especialidad.Activo = false;
            await _especialidadRepository.UpdateAsync(especialidad);
        }

        // Convierte una entidad Especialidad a su DTO de respuesta
        private static EspecialidadResponse MapToResponse(Especialidad e)
        {
            return new EspecialidadResponse
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Activo = e.Activo
            };
        }
    }
}
