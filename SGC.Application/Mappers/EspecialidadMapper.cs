using SGC.Application.DTOs.Catalog;
using SGC.Domain.Entities.Catalog;

namespace SGC.Application.Mappers
{
    public static class EspecialidadMapper
    {
        public static EspecialidadResponse ToResponse(
            Especialidad especialidad)
        {
            return new EspecialidadResponse
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion,
                Activo = especialidad.Activo
            };
        }

        public static Especialidad ToEntity(EspecialidadRequest request)
        {
            return new Especialidad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion
            };
        }
    }
}