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
    private EstadoCita _estadoFiltro = EstadoCita.Confirmada;
    private DateTime _fechaFiltro = DateTime.Today;
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
                _ = CargarCitas();
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
        ConfirmarCitaCommand = new Command(async () => await ConfirmarCita());
        CompletarCitaCommand = new Command(async () => await CompletarCita());
        CancelarCitaCommand = new Command(async () => await CancelarCitaSeleccionada());
        ActualizarEstadoCommand = new Command<EstadoCita>(async (estado) => await ActualizarEstadoCita(estado));

        _ = CargarCitas();
    }

    private async Task CargarCitas()
    {
        IsBusy = true;
        try
        {
            var agenda = await _citasService.ObtenerAgendaAsync(FechaFiltro);
            var citasDto = agenda.Citas ?? new List<CitaResponseDto>();

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

    private async Task ConfirmarCita()
    {
        if (CitaSeleccionada == null) return;

        await _citasService.ConfirmarCitaAsync(CitaSeleccionada.Id, true);
        await CargarCitas();
    }

    private async Task CompletarCita()
    {
        if (CitaSeleccionada == null) return;

        await _citasService.MarcarAsistenciaAsync(CitaSeleccionada.Id, true);
        await CargarCitas();
    }

    private async Task CancelarCitaSeleccionada()
    {
        if (CitaSeleccionada == null) return;

        await _citasService.MarcarAsistenciaAsync(CitaSeleccionada.Id, false);
        await CargarCitas();
    }

    private async Task ActualizarEstadoCita(EstadoCita nuevoEstado)
    {
        if (CitaSeleccionada == null) return;

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

    private void AplicarFiltros()
    {
        var filtradas = Citas.Where(c =>
            c.Estado == EstadoFiltro &&
            c.FechaHora.Date == FechaFiltro.Date &&
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
        var estado = dto.Estado.ToLower() switch
        {
            "confirmada" => EstadoCita.Confirmada,
            "completada" => EstadoCita.Completada,
            "cancelada" => EstadoCita.Cancelada,
            "rechazada" => EstadoCita.Cancelada,
            "noasistio" => EstadoCita.Cancelada,
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
