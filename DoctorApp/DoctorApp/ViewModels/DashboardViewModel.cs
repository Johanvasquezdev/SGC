using System.Collections.ObjectModel;
using System.Windows.Input;
using DoctorApp.Models;
using DoctorApp.Services.Interfaces;
using DoctorApp.Services.Hubs;
using DoctorApp.Exceptions;
using DoctorApp.DTOs.Responses;

namespace DoctorApp.ViewModels;

/// <summary>
/// ViewModel del Dashboard - Gestión de doctores, citas y búsqueda de pacientes
/// Implementa cambio dinámico de doctor, carga de citas reales y búsqueda por cédula
/// </summary>
public class DashboardViewModel : BaseViewModel
{
    private Medico _medicoActual = new();
    private ObservableCollection<Medico> _doctoresDisponibles = new();
    private ObservableCollection<Cita> _citasHoy = new();
    private ObservableCollection<Cita> _citasProximas = new();
    private int _totalCitasHoy;
    private int _citasConfirmadas;
    private int _citasPendientes;
    private DateTime _fechaSeleccionada = DateTime.Today;
    private string _busquedaCedula = string.Empty;
    private Paciente? _pacienteBuscado;
    private string _mensajeBusqueda = string.Empty;

    // Propiedades del ViewModel
    public Medico MedicoActual
    {
        get => _medicoActual;
        set
        {
            if (_medicoActual != value)
            {
                _medicoActual = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Medico> DoctoresDisponibles
    {
        get => _doctoresDisponibles;
        set
        {
            if (_doctoresDisponibles != value)
            {
                _doctoresDisponibles = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Cita> CitasHoy
    {
        get => _citasHoy;
        set
        {
            if (_citasHoy != value)
            {
                _citasHoy = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Cita> CitasProximas
    {
        get => _citasProximas;
        set
        {
            if (_citasProximas != value)
            {
                _citasProximas = value;
                OnPropertyChanged();
            }
        }
    }

    public int TotalCitasHoy
    {
        get => _totalCitasHoy;
        set
        {
            if (_totalCitasHoy != value)
            {
                _totalCitasHoy = value;
                OnPropertyChanged();
            }
        }
    }

    public int CitasConfirmadas
    {
        get => _citasConfirmadas;
        set
        {
            if (_citasConfirmadas != value)
            {
                _citasConfirmadas = value;
                OnPropertyChanged();
            }
        }
    }

    public int CitasPendientes
    {
        get => _citasPendientes;
        set
        {
            if (_citasPendientes != value)
            {
                _citasPendientes = value;
                OnPropertyChanged();
            }
        }
    }

    public string BusquedaCedula
    {
        get => _busquedaCedula;
        set
        {
            if (_busquedaCedula != value)
            {
                _busquedaCedula = value;
                OnPropertyChanged();
            }
        }
    }

    public Paciente? PacienteBuscado
    {
        get => _pacienteBuscado;
        set
        {
            if (_pacienteBuscado != value)
            {
                _pacienteBuscado = value;
                OnPropertyChanged();
            }
        }
    }

    public string MensajeBusqueda
    {
        get => _mensajeBusqueda;
        set
        {
            if (_mensajeBusqueda != value)
            {
                _mensajeBusqueda = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime FechaSeleccionada
    {
        get => _fechaSeleccionada;
        set
        {
            if (_fechaSeleccionada != value)
            {
                _fechaSeleccionada = value;
                OnPropertyChanged();
                CargarCitasDelDia();
            }
        }
    }

    // Commands
    public ICommand CargarDashboardCommand { get; }
    public ICommand RefrescarCommand { get; }
    public ICommand SeleccionarDoctorCommand { get; }
    public ICommand BuscarPacientePorCedulaCommand { get; }
    public ICommand LimpiarBusquedaCommand { get; }

    // Servicios
    private readonly ICitasService _citasService;
    private readonly IDoctorService _doctorService;
    private readonly ICitasHubClient _citasHubClient;

    public DashboardViewModel(
        ICitasService citasService,
        IDoctorService doctorService,
        ICitasHubClient citasHubClient)
    {
        Title = "Panel de Control";

        _citasService = citasService ?? throw new ArgumentNullException(nameof(citasService));
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _citasHubClient = citasHubClient ?? throw new ArgumentNullException(nameof(citasHubClient));

        // Inicializar commands
        CargarDashboardCommand = new Command(async () => await CargarDashboard());
        RefrescarCommand = new Command(async () => await CargarDashboard());
        SeleccionarDoctorCommand = new Command<Medico>(async (doctor) => await SeleccionarDoctor(doctor));
        BuscarPacientePorCedulaCommand = new Command(async () => await BuscarPacientePorCedula());
        LimpiarBusquedaCommand = new Command(LimpiarBusqueda);

        // Suscribirse a eventos del hub
        _citasHubClient.OnNuevaEnCita += OnNuevaEnCita;
        _citasHubClient.OnCitaConfirmada += OnCitaConfirmada;

        // Cargar datos iniciales
        _ = CargarDatosIniciales();

        // Conectar al hub SignalR
        _ = ConectarAlHub();
    }

    /// <summary>
    /// Carga los datos iniciales: lista de doctores y selecciona el primero
    /// </summary>
    private async Task CargarDatosIniciales()
    {
        InicializarDoctoresDisponibles();

        if (DoctoresDisponibles.Count > 0)
        {
            await SeleccionarDoctor(DoctoresDisponibles[0]);
        }
    }

    /// <summary>
    /// Inicializa la lista de doctores disponibles (Dr. Carlos García, Dr. Manuel Gómez, Dra. Maria Fernández)
    /// </summary>
    private void InicializarDoctoresDisponibles()
    {
        DoctoresDisponibles.Clear();

        DoctoresDisponibles.Add(new Medico
        {
            Id = 1,
            Nombre = "Carlos",
            Apellido = "García",
            Especialidad = "Cardiología",
            Consultorio = "201",
            Email = "carlos.garcia@hospital.com",
            Telefono = "+34 600 111 222",
            NumeroLicencia = "LIC001",
            Activo = true,
            FechaRegistro = DateTime.Now
        });

        DoctoresDisponibles.Add(new Medico
        {
            Id = 2,
            Nombre = "Manuel",
            Apellido = "Gómez",
            Especialidad = "Neurología",
            Consultorio = "202",
            Email = "manuel.gomez@hospital.com",
            Telefono = "+34 600 333 444",
            NumeroLicencia = "LIC002",
            Activo = true,
            FechaRegistro = DateTime.Now
        });

        DoctoresDisponibles.Add(new Medico
        {
            Id = 3,
            Nombre = "Maria",
            Apellido = "Fernández",
            Especialidad = "Pediatría",
            Consultorio = "203",
            Email = "maria.fernandez@hospital.com",
            Telefono = "+34 600 555 666",
            NumeroLicencia = "LIC003",
            Activo = true,
            FechaRegistro = DateTime.Now
        });

        System.Diagnostics.Debug.WriteLine("[DashboardViewModel] Doctores inicializados: 3 doctores cargados");
    }

    /// <summary>
    /// Selecciona un doctor y carga sus citas reales desde la API
    /// </summary>
    private async Task SeleccionarDoctor(Medico doctor)
    {
        if (doctor == null)
            return;

        IsBusy = true;
        try
        {
            // Establecer el doctor seleccionado
            MedicoActual = doctor;
            _doctorService.EstablecerDoctorId(doctor.Id);

            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Doctor seleccionado: {doctor.NombreCompleto} (ID: {doctor.Id})");

            // Cargar citas reales del doctor desde la API usando DoctorService
            var citas = await _doctorService.GetCitasByDoctorIdAsync(doctor.Id);

            if (citas != null && citas.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Citas cargadas: {citas.Count} total");

                // Separar citas de hoy
                CitasHoy = new ObservableCollection<Cita>(
                    citas
                        .Where(c => c.FechaHora.Date == DateTime.Today)
                        .Select(MapearCitaDtoACita)
                        .ToList()
                );

                // Separar citas próximas no confirmadas
                CitasProximas = new ObservableCollection<Cita>(
                    citas
                        .Where(c => c.FechaHora.Date > DateTime.Today && !c.Confirmada)
                        .Select(MapearCitaDtoACita)
                        .ToList()
                );

                System.Diagnostics.Debug.WriteLine(
                    $"[DashboardViewModel] Citas de hoy: {CitasHoy.Count}, Próximas: {CitasProximas.Count}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] No hay citas para el doctor {doctor.NombreCompleto}");
                CitasHoy.Clear();
                CitasProximas.Clear();
            }

            // Actualizar estadísticas
            ActualizarEstadisticas();
        }
        catch (AppException ex) when (ex.Code == "DOCTOR_NOT_FOUND")
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Doctor no encontrado en BD: ID {doctor.Id}");
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"El doctor {doctor.NombreCompleto} no fue encontrado en el sistema",
                "OK");
            CitasHoy.Clear();
            CitasProximas.Clear();
            ActualizarEstadisticas();
        }
        catch (ConnectionException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Error de conexión: {ex.Message}");
            await Application.Current!.MainPage!.DisplayAlert(
                "Error de Conexión",
                "No se pudo obtener las citas del doctor. Intenta más tarde.",
                "Reintentar");
            CitasHoy.Clear();
            CitasProximas.Clear();
            ActualizarEstadisticas();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Error inesperado: {ex.Message}");
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                $"Ocurrió un error: {ex.Message}",
                "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Busca un paciente por cédula invocando el endpoint correspondiente
    /// Asigna el correo electrónico del paciente al campo PacienteBuscado mediante Data Binding
    /// </summary>
    private async Task BuscarPacientePorCedula()
    {
        if (string.IsNullOrWhiteSpace(BusquedaCedula))
        {
            MensajeBusqueda = "Por favor ingresa una cédula";
            PacienteBuscado = null;
            return;
        }

        IsBusy = true;
        MensajeBusqueda = string.Empty;

        try
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Buscando paciente con cédula: {BusquedaCedula}");

            // Buscar paciente en la API (simulado por ahora, en tu API real sería un endpoint GET /api/pacientes/cedula/{cedula})
            PacienteBuscado = await BuscarPacienteEnAPI(BusquedaCedula);

            if (PacienteBuscado != null)
            {
                MensajeBusqueda = $"✓ Paciente encontrado: {PacienteBuscado.NombreCompleto}";
                System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Paciente encontrado - Email: {PacienteBuscado.Email}");
            }
            else
            {
                MensajeBusqueda = "✗ No se encontró paciente con esa cédula";
                PacienteBuscado = null;
            }
        }
        catch (AppException ex) when (ex.Code == "NOT_FOUND")
        {
            MensajeBusqueda = "✗ Paciente no encontrado";
            PacienteBuscado = null;
        }
        catch (ConnectionException ex)
        {
            MensajeBusqueda = "✗ Error de conexión. Intenta más tarde.";
            PacienteBuscado = null;
        }
        catch (Exception ex)
        {
            MensajeBusqueda = $"✗ Error: {ex.Message}";
            PacienteBuscado = null;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Simula la búsqueda de paciente por cédula en la API
    /// En tu backend, este sería un endpoint: GET /api/pacientes/cedula/{cedula}
    /// </summary>
    private async Task<Paciente?> BuscarPacienteEnAPI(string cedula)
    {
        // NOTA: Esto es un mock para demostración
        // En tu API real, reemplaza esto con:
        // return await _apiClient.GetAsync<Paciente>($"/api/pacientes/cedula/{cedula}");

        var pacientesMock = new List<Paciente>
        {
            new Paciente
            {
                Id = 101,
                Nombre = "Juan",
                Apellido = "Pérez",
                Cedula = "1234567890",
                Email = "juan.perez@email.com",
                Telefono = "+34 600 123 456",
                FechaNacimiento = new DateTime(1990, 5, 15),
                Genero = "M",
                Direccion = "Calle Principal 123"
            },
            new Paciente
            {
                Id = 102,
                Nombre = "María",
                Apellido = "González",
                Cedula = "0987654321",
                Email = "maria.gonzalez@email.com",
                Telefono = "+34 600 987 654",
                FechaNacimiento = new DateTime(1992, 8, 22),
                Genero = "F",
                Direccion = "Avenida Central 456"
            },
            new Paciente
            {
                Id = 103,
                Nombre = "Carlos",
                Apellido = "López",
                Cedula = "5555555555",
                Email = "carlos.lopez@email.com",
                Telefono = "+34 600 555 999",
                FechaNacimiento = new DateTime(1988, 3, 10),
                Genero = "M",
                Direccion = "Pasaje Este 789"
            }
        };

        // Simular latencia de red
        await Task.Delay(500);

        return pacientesMock.FirstOrDefault(p => p.Cedula == cedula);
    }

    /// <summary>
    /// Limpia los campos de búsqueda
    /// </summary>
    private void LimpiarBusqueda()
    {
        BusquedaCedula = string.Empty;
        PacienteBuscado = null;
        MensajeBusqueda = string.Empty;
    }

    /// <summary>
    /// Conecta al hub SignalR para actualizaciones en tiempo real
    /// </summary>
    private async Task ConectarAlHub()
    {
        try
        {
            await _citasHubClient.ConnectAsync();
        }
        catch (UnauthorizedException)
        {
            // Token inválido o no autenticado
            await Application.Current!.MainPage!.DisplayAlert(
                "Error",
                "No autenticado. Inicie sesión nuevamente.",
                "OK");
        }
        catch (ConnectionException ex)
        {
            // Conexión fallo, intentaremos reconectar automáticamente
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] SignalR connection failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Evento cuando hay una nueva cita (SignalR)
    /// </summary>
    private void OnNuevaEnCita(object? sender, Services.Hubs.CitaHubEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (MedicoActual != null)
            {
                _ = SeleccionarDoctor(MedicoActual);
            }
        });
    }

    /// <summary>
    /// Evento cuando se confirma una cita (SignalR)
    /// </summary>
    private void OnCitaConfirmada(object? sender, Services.Hubs.CitaHubEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (MedicoActual != null)
            {
                _ = SeleccionarDoctor(MedicoActual);
            }
        });
    }

    /// <summary>
    /// Carga las citas del día cuando cambia la fecha seleccionada
    /// </summary>
    private void CargarCitasDelDia()
    {
        if (MedicoActual != null && MedicoActual.Id > 0)
        {
            _ = SeleccionarDoctor(MedicoActual);
        }
    }

    /// <summary>
    /// Carga el dashboard completo (método legacy, mantenido por compatibilidad)
    /// </summary>
    private async Task CargarDashboard()
    {
        IsBusy = true;
        try
        {
            if (MedicoActual != null && MedicoActual.Id > 0)
            {
                await SeleccionarDoctor(MedicoActual);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Actualiza las estadísticas de citas (Total, Confirmadas, Pendientes)
    /// </summary>
    private void ActualizarEstadisticas()
    {
        TotalCitasHoy = CitasHoy.Count;
        CitasConfirmadas = CitasHoy.Count(c => c.Confirmada);
        CitasPendientes = CitasHoy.Count(c => !c.Confirmada);
    }

    /// <summary>
    /// Mapea un CitaResponseDto a un modelo Cita para la UI
    /// </summary>
    private Cita MapearCitaDtoACita(CitaResponseDto citaDto)
    {
        return new Cita
        {
            Id = citaDto.Id,
            FechaHora = citaDto.FechaHora,
            Motivo = citaDto.Motivo,
            Confirmada = citaDto.Confirmada,
            Paciente = new Paciente
            {
                Id = citaDto.PacienteId,
                Nombre = citaDto.PacienteNombre,
                Cedula = citaDto.PacienteCedula
            },
            Estado = citaDto.Confirmada ? EstadoCita.Confirmada : EstadoCita.Pendiente,
            DuracionMinutos = citaDto.DuracionMinutos
        };
    }

    /// <summary>
    /// Desconecta del hub SignalR cuando el ViewModel se destruye
    /// </summary>
    public async Task OnDisappearing()
    {
        try
        {
            if (_citasHubClient.IsConnected)
                await _citasHubClient.DisconnectAsync();
        }
        catch { /* Ignore */ }
    }
}
