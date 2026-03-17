using SGC.Application.DTOs.Audit;
using SGC.Domain.Base;

namespace SGC.Application.Mappers
{
    public static class AuditoriaMapper
    {
        public static AuditoriaResponse ToResponse(AuditEntity auditEntity)
        {
            return new AuditoriaResponse
            {
                Id = auditEntity.Id,
                UsuarioId = auditEntity.UsuarioId,
                Entidad = auditEntity.Entidad,
                Accion = auditEntity.Accion,
                ValorAnterior = auditEntity.ValorAnterior,
                ValorNuevo = auditEntity.ValorNuevo,
                Fecha = auditEntity.Fecha,
                DireccionIP = auditEntity.DireccionIP
            };
        }
    }
}