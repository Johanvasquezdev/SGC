// SGC.Application/Services/AuditoriaService.cs
using SGC.Application.Contracts;
using SGC.Application.DTOs.Audit;
using SGC.Application.Mappers;
using SGC.Application.Services.Base;
using SGC.Domain.Base;
using SGC.Domain.Interfaces.ILogger;
using SGC.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGC.Application.Services
{
    public class AuditoriaService : BaseService, IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaService(
            IAuditoriaRepository auditoriaRepository,
            ISGCLogger logger) : base(logger)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task RegistrarAsync(int? usuarioId, string entidad,
            string accion, string? valorAnterior,
            string? valorNuevo, string? direccionIP)
        {
            LogOperacion("RegistrarAuditoria",
                $"Entidad: {entidad}, Accion: {accion}");

            var evento = new AuditEntity
            {
                UsuarioId = usuarioId,
                Entidad = entidad,
                Accion = accion,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                DireccionIP = direccionIP,
                Fecha = DateTime.UtcNow
            };

            await _auditoriaRepository.AddAsync(evento);
        }

        public async Task<IEnumerable<AuditoriaResponse>> GetByEntidadAsync(
            string entidad)
        {
            var eventos = await _auditoriaRepository
                .GetByEntidadAsync(entidad);
            return eventos.Select(AuditoriaMapper.ToResponse);
        }

        public async Task<IEnumerable<AuditoriaResponse>> GetByUsuarioAsync(
            int usuarioId)
        {
            var eventos = await _auditoriaRepository
                .GetByUsuarioIdAsync(usuarioId);
            return eventos.Select(AuditoriaMapper.ToResponse);
        }
    }
}