using System.Collections.ObjectModel;
using System.Windows.Input;
using DoctorApp.Models;
using DoctorApp.Services.Interfaces;
using DoctorApp.DTOs.Responses;
using DoctorApp.Exceptions;

namespace DoctorApp.ViewModels;

public class GestionCitasViewModel : BaseViewModel
{
    private ObservableCollection<Cita> _citas = new();
    private ObservableCollection<Cita> _citasFiltradas = new();
    private Cita? _citaSeleccionada;
    private EstadoCita _estadoFiltro = EstadoCita.Todos;
    private DateTime _fechaFiltro = DateTime.Today;
    private bool _usarFechaFiltro;
    private string _busquedaPaciente = string.Empty;
    private bool _mostrarCancelar;

    public List<EstadoCita> EstadosDisponibles => Enum.GetValues(typeof(EstadoCita))
        .Cast<EstadoCita>()
        .ToList();

    public ObservableCollection<Cita> Citas
    {
        get => _citas;
        set
        {
            if (_citas != value)
            {
                _citas = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Cita> CitasFiltradas
    {
        get => _citasFiltradas;
        set
        {
            if (_citasFiltradas != value)
            {
                _citasFiltradas = value;
                OnPropertyChanged();
            }
        }
    }

    public Cita? CitaSeleccionada
    {
        get => _citaSeleccionada;
        set
        {
            if (_citaSeleccionada != value)
            {
                _citaSeleccionada = value;
                OnPropertyChanged();
            }
        }
    }

    public EstadoCita EstadoFiltro
    {
        get => _estadoFiltro;
        set
        {
            if (_estadoFiltro != value)
            {
                _estadoFiltro = value;
                OnPropertyChanged();
                AplicarFiltros();
            }
        }
    }

    public DateTime FechaFiltro
    {
        get => _fechaFiltro;
        set
        {
            if (_fechaFiltro != value)
            {
                _fechaFiltro = value;
                OnPropertyChanged();
                if (UsarFechaFiltro)
                    AplicarFiltros();
            }
        }
    }

    public bool UsarFechaFiltro
    {
        get => _usarFechaFiltro;
        set
        {
            if (_usarFechaFiltro != value)
            {
                _usarFechaFiltro = value;
                OnPropertyChanged();
                AplicarFiltros();
            }
        }
    }

    public string BusquedaPaciente
    {
        get => _busquedaPaciente;
        set
        {
            if (_busquedaPaciente != value)
            {
                _busquedaPaciente = value;
                OnPropertyChanged();
                AplicarFiltros();
            }
        }
    }

    public bool MostrarCancelar
    {
        get => _mostrarCancelar;
        set
        {
            if (_mostrarCancelar != value)
            {
                _mostrarCancelar = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand CargarCitasCommand { get; }
    public ICommand ConfirmarCitaCommand { get; }
    public ICommand CompletarCitaCommand { get; }
    public ICommand CancelarCitaCommand { get; }
    public ICommand ActualizarEstadoCommand { get; }

    private readonly ICitasService _citasService;

    public GestionCitasViewModel(ICitasService citasService)
    {
        _citasService = citasService ?? throw new ArgumentNullException(nameof(citasService));
        Title = "Gestión de Citas";

        CargarCitasCommand = new Command(async () => await CargarCitas());
        ConfirmarCitaCommand = new Command<Cita>(async (cita) => await ConfirmarCita(cita));
        CompletarCitaCommand = new Command<Cita>(async (cita) => await CompletarCita(cita));
        CancelarCitaCommand = new Command<Cita>(async (cita) => await CancelarCitaSeleccionada(cita));
        ActualizarEstadoCommand = new Command<EstadoCita>(async (estado) => await ActualizarEstadoCita(estado));

        _ = CargarCitas();
    }

    private async Task CargarCitas()
    {
        IsBusy = true;
        try
        {
            var citasDto = await _citasService.ObtenerCitasMedicoAsync();
            System.Diagnostics.Debug.WriteLine($"[GestionCitas] API citas recibidas: {citasDto?.Count ?? 0}");
            await Application.Current!.MainPage!.DisplayAlert(
                "Debug Citas",
                $"Citas recibidas desde API: {citasDto?.Count ?? 0}",
                "OK");

            Citas.Clear();
            foreach (var dto in citasDto)
            {
                Citas.Add(MapearCita(dto));
            }

            AplicarFiltros();
        }
        catch (UnauthorizedException)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No autenticado. Inicie sesión nuevamente.", "OK");
        }
        catch (ConnectionException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Conexión", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ConfirmarCita(Cita? cita)
    {
        var citaObjetivo = cita ?? CitaSeleccionada;
        if (citaObjetivo == null) return;

        try
        {
            await _citasService.ConfirmarCitaAsync(citaObjetivo.Id, true);
            await CargarCitas();
        }
        catch (ValidationException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Validacion", ex.Message, "OK");
        }
        catch (ConnectionException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Conexion", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task CompletarCita(Cita? cita)
    {
        var citaObjetivo = cita ?? CitaSeleccionada;
        if (citaObjetivo == null) return;

        try
        {
            await _citasService.MarcarAsistenciaAsync(citaObjetivo.Id, true);
            await CargarCitas();
        }
        catch (ValidationException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Validacion", ex.Message, "OK");
        }
        catch (ConnectionException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Conexion", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task CancelarCitaSeleccionada(Cita? cita)
    {
        var citaObjetivo = cita ?? CitaSeleccionada;
        if (citaObjetivo == null) return;

        try
        {
            await _citasService.MarcarAsistenciaAsync(citaObjetivo.Id, false);
            await CargarCitas();
        }
        catch (ValidationException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Validacion", ex.Message, "OK");
        }
        catch (ConnectionException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Conexion", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task ActualizarEstadoCita(EstadoCita nuevoEstado)
    {
        if (CitaSeleccionada == null) return;

        try
        {
            if (nuevoEstado == EstadoCita.Confirmada)
            {
                await _citasService.ConfirmarCitaAsync(CitaSeleccionada.Id, true);
            }
            else if (nuevoEstado == EstadoCita.Completada)
            {
                await _citasService.MarcarAsistenciaAsync(CitaSeleccionada.Id, true);
            }
            else if (nuevoEstado == EstadoCita.Cancelada)
            {
                await _citasService.MarcarAsistenciaAsync(CitaSeleccionada.Id, false);
            }

            await CargarCitas();
        }
        catch (ValidationException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Validacion", ex.Message, "OK");
        }
        catch (ConnectionException ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error de Conexion", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void AplicarFiltros()
    {
        var filtradas = Citas.Where(c =>
            (EstadoFiltro == EstadoCita.Todos || c.Estado == EstadoFiltro) &&
            (!UsarFechaFiltro || c.FechaHora.Date == FechaFiltro.Date) &&
            (string.IsNullOrEmpty(BusquedaPaciente) ||
             c.Paciente?.NombreCompleto?.Contains(BusquedaPaciente, StringComparison.OrdinalIgnoreCase) == true ||
             c.Paciente?.Cedula?.Contains(BusquedaPaciente) == true)
        ).OrderBy(c => c.FechaHora).ToList();

        CitasFiltradas.Clear();
        foreach (var cita in filtradas)
        {
            CitasFiltradas.Add(cita);
        }
    }

    private static Cita MapearCita(CitaResponseDto dto)
    {
        System.Diagnostics.Debug.WriteLine($"[GestionCitas] Mapeando cita {dto.Id} paciente {dto.PacienteNombre} fecha {dto.FechaHora}");
        var estado = dto.Estado.ToLower() switch
        {
            "confirmada" => EstadoCita.Confirmada,
            "completada" => EstadoCita.Completada,
            "cancelada" => EstadoCita.Cancelada,
            "rechazada" => EstadoCita.Cancelada,
            "noasistio" => EstadoCita.NoAsistio,
            _ => EstadoCita.Pendiente
        };

        return new Cita
        {
            Id = dto.Id,
            FechaHora = dto.FechaHora,
            Motivo = dto.Motivo ?? string.Empty,
            Confirmada = dto.Confirmada,
            Estado = estado,
            DuracionMinutos = dto.DuracionMinutos,
            Paciente = new Paciente
            {
                Id = dto.PacienteId,
                Nombre = dto.PacienteNombre,
                Cedula = string.Empty
            }
        };
    }
}
