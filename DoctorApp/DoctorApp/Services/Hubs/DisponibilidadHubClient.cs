using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using DoctorApp.Security;
using DoctorApp.Exceptions;

namespace DoctorApp.Services.Hubs
{
    /// <summary>
    /// Cliente SignalR para el hub de disponibilidad
    /// Administra conexión a tiempo real con backend para eventos de disponibilidad
    /// </summary>
    public class DisponibilidadHubClient : IDisponibilidadHubClient
    {
        private HubConnection? _hubConnection;
        private readonly ITokenManager _tokenManager;
        private const string HubEndpoint = "http://localhost:5189/disponibilidadhub";

        public event EventHandler<DisponibilidadHubEventArgs>? OnDisponibilidadCreada;
        public event EventHandler<DisponibilidadHubEventArgs>? OnDisponibilidadActualizada;
        public event EventHandler<DisponibilidadHubEventArgs>? OnDisponibilidadEliminada;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public DisponibilidadHubClient(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        /// <summary>
        /// Conecta al hub de disponibilidad de forma asincrónica
        /// Configura handlers para los eventos del servidor
        /// </summary>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Evitar múltiples conexiones
                if (_hubConnection?.State == HubConnectionState.Connected)
                    return;

                // Obtener token para autenticación
                var token = await _tokenManager.GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedException("No hay token de autenticación disponible");

                // Crear conexión con bearer token
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(HubEndpoint, options =>
                    {
                        options.AccessTokenProvider = async () => token;
                    })
                    .WithAutomaticReconnect(new[] 
                    { 
                        TimeSpan.Zero,
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(10)
                    })
                    .Build();

                // Registrar handlers de eventos del servidor
                _hubConnection.On<int, string, string, string, int, bool>("DisponibilidadCreada", 
                    (dispId, dia, horaInicio, horaFin, duracion, activo) =>
                    {
                        OnDisponibilidadCreada?.Invoke(this, new DisponibilidadHubEventArgs
                        {
                            DisponibilidadId = dispId,
                            DiaSemana = dia,
                            HoraInicio = TimeSpan.TryParse(horaInicio, out var hi) ? hi : TimeSpan.Zero,
                            HoraFin = TimeSpan.TryParse(horaFin, out var hf) ? hf : TimeSpan.Zero,
                            DuracionMinutos = duracion,
                            Activo = activo
                        });
                    });

                _hubConnection.On<int, bool>("DisponibilidadActualizada", 
                    (dispId, activo) =>
                    {
                        OnDisponibilidadActualizada?.Invoke(this, new DisponibilidadHubEventArgs
                        {
                            DisponibilidadId = dispId,
                            Activo = activo
                        });
                    });

                _hubConnection.On<int>("DisponibilidadEliminada", 
                    (dispId) =>
                    {
                        OnDisponibilidadEliminada?.Invoke(this, new DisponibilidadHubEventArgs
                        {
                            DisponibilidadId = dispId
                        });
                    });

                // Handlers de reconexión
                _hubConnection.Reconnecting += error =>
                {
                    Debug.WriteLine($"[DisponibilidadHubClient] Reconnecting: {error?.Message}");
                    return Task.CompletedTask;
                };

                _hubConnection.Reconnected += connectionId =>
                {
                    Debug.WriteLine($"[DisponibilidadHubClient] Reconnected: {connectionId}");
                    return Task.CompletedTask;
                };

                _hubConnection.Closed += error =>
                {
                    Debug.WriteLine($"[DisponibilidadHubClient] Connection closed: {error?.Message}");
                    return Task.CompletedTask;
                };

                // Conectar al hub
                await _hubConnection.StartAsync(cancellationToken);
                Debug.WriteLine("[DisponibilidadHubClient] Connected successfully");
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"Error al conectar al hub de disponibilidad: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Desconecta del hub de disponibilidad
        /// </summary>
        public async Task DisconnectAsync()
        {
            try
            {
                if (_hubConnection != null)
                {
                    await _hubConnection.StopAsync();
                    await _hubConnection.DisposeAsync();
                    _hubConnection = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DisponibilidadHubClient] Error during disconnect: {ex.Message}");
            }
        }
    }
}
