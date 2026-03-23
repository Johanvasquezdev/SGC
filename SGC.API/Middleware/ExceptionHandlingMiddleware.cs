using System.Net;
using System.Text.Json;
using SGC.Domain.Exceptions;

namespace SGC.API.Middleware
{
    // Middleware global para capturar excepciones y devolver respuestas HTTP consistentes
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        // Mapea cada tipo de excepcion a su codigo HTTP correspondiente
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, mensaje) = exception switch
            {
                // 400 - Solicitud invalida (validaciones, argumentos incorrectos)
                InvalidOperationException ex => (HttpStatusCode.BadRequest, ex.Message),
                ArgumentException ex => (HttpStatusCode.BadRequest, ex.Message),

                // 404 - Recurso no encontrado
                CitaNotFoundException ex => (HttpStatusCode.NotFound, ex.Message),
                KeyNotFoundException ex => (HttpStatusCode.NotFound, ex.Message),

                // 409 - Conflicto (citas duplicadas, horario no disponible)
                CitaConflictoException ex => (HttpStatusCode.Conflict, ex.Message),
                HorarioNoDisponibleException ex => (HttpStatusCode.Conflict, ex.Message),

                // 401 - No autorizado
                UnauthorizedAccessException ex => (HttpStatusCode.Unauthorized, ex.Message),

                // 500 - Error interno del servidor
                _ => (HttpStatusCode.InternalServerError, "Ocurrio un error interno en el servidor.")
            };

            // Solo registrar errores internos como Error; los demas como Warning
            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Error interno: {Mensaje}", exception.Message);
            else
                _logger.LogWarning("Excepcion controlada ({StatusCode}): {Mensaje}", (int)statusCode, mensaje);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                status = (int)statusCode,
                error = statusCode.ToString(),
                mensaje
            };

            var json = JsonSerializer.Serialize(respuesta, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
