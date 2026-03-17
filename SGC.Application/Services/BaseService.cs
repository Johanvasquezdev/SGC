
using SGC.Domain.Interfaces.ILogger;
using System;

namespace SGC.Application.Services.Base
{
    // Clase base para todos los servicios de la aplicacion, proporcionando acceso al logger y metodos comunes de log.
    public abstract class BaseService
    {
        protected readonly ISGCLogger Logger;

        protected BaseService(ISGCLogger logger)
        {
            Logger = logger;
        }

        // Metodos de log comunes para las operaciones de los servicios, que incluyen el nombre del servicio en el mensaje para facilitar la identificacion en los logs.
        protected void LogOperacion(string operacion, string detalle) =>
            Logger.LogInfo($"[{GetType().Name}] {operacion}: {detalle}");

        protected void LogAdvertencia(string operacion, string detalle) =>
            Logger.LogWarning($"[{GetType().Name}] {operacion}: {detalle}");

        protected void LogError(string operacion, Exception ex) =>
            Logger.LogError($"[{GetType().Name}] Error en {operacion}", ex);
    }
}