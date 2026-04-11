using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using DoctorApp.Security;
using DoctorApp.Exceptions;

namespace DoctorApp.Services.Hubs
{
    /// <summary>
    /// Cliente SignalR para el hub de citas
    /// Administra conexión a tiempo real con backend para eventos de citas
    /// </summary>
    public class CitasHubClient : ICitasHubClient
    {
        private HubConnection? _hubConnection;
        private readonly ITokenManager _tokenManager;
        private const string HubEndpoint = "http://localhost:5189/citahub";

        public event EventHandler<CitaHubEventArgs>? OnNuevaEnCita;
        public event EventHandler<CitaHubEventArgs>? OnCitaActualizada;
        public event EventHandler<CitaHubEventArgs>? OnCitaConfirmada;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public CitasHubClient(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        /// <summary>
        /// Conecta al hub de citas de forma asincrónica
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
                _hubConnection.On<int, string, string, string>("NuevaEnCita", 
                    (citaId, pacienteNombre, fechaHora, motivo) =>
                    {
                        OnNuevaEnCita?.Invoke(this, new CitaHubEventArgs
                        {
                            CitaId = citaId,
                            PacienteNombre = pacienteNombre,
                            Estado = "Nueva",
                            FechaHora = DateTime.TryParse(fechaHora, out var dt) ? dt : DateTime.UtcNow
                        });
                    });

                _hubConnection.On<int, string>("CitaActualizada", 
                    (citaId, nuevoEstado) =>
                    {
                        OnCitaActualizada?.Invoke(this, new CitaHubEventArgs
                        {
                            CitaId = citaId,
                            Estado = nuevoEstado
                        });
                    });

                _hubConnection.On<int>("CitaConfirmada", 
                    (citaId) =>
                    {
                        OnCitaConfirmada?.Invoke(this, new CitaHubEventArgs
                        {
                            CitaId = citaId,
                            Estado = "Confirmada"
                        });
                    });

                // Handlers de reconexión
                _hubConnection.Reconnecting += error =>
                {
                    Debug.WriteLine($"[CitasHubClient] Reconnecting: {error?.Message}");
                    return Task.CompletedTask;
                };

                _hubConnection.Reconnected += connectionId =>
                {
                    Debug.WriteLine($"[CitasHubClient] Reconnected: {connectionId}");
                    return Task.CompletedTask;
                };

                _hubConnection.Closed += error =>
                {
                    Debug.WriteLine($"[CitasHubClient] Connection closed: {error?.Message}");
                    return Task.CompletedTask;
                };

                // Conectar al hub
                await _hubConnection.StartAsync(cancellationToken);
                Debug.WriteLine("[CitasHubClient] Connected successfully");
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConnectionException($"Error al conectar al hub de citas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Desconecta del hub de citas
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
                Debug.WriteLine($"[CitasHubClient] Error during disconnect: {ex.Message}");
            }
        }
    }
}
