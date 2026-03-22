using SGC.Domain.Interfaces.ILogger;
using System;

namespace SGC.Infraestructure.Logging
{
    // Logger básico para desarrollo que escribe en consola.
    public class SGCLogger : ISGCLogger
    {
        // Registra mensajes informativos.
        public void LogInfo(string mensaje) => Console.WriteLine($"[INFO] {mensaje}");

        // Registra advertencias.
        public void LogWarning(string mensaje) => Console.WriteLine($"[WARN] {mensaje}");

        // Registra errores y su excepción asociada si existe.
        public void LogError(string mensaje, Exception? ex = null) =>
            Console.WriteLine($"[ERROR] {mensaje} {ex?.Message}");

        // Registra mensajes de depuración.
        public void LogDebug(string mensaje) => Console.WriteLine($"[DEBUG] {mensaje}");
    }
}
