using System.Collections.ObjectModel;
using System.Windows.Input;
using DoctorApp.Models;

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

    public GestionCitasViewModel()
    {
        Title = "Gestión de Citas";
        CargarCitasCommand = new Command(async () => await CargarCitas());
        ConfirmarCitaCommand = new Command(async () => await ConfirmarCita());
        CompletarCitaCommand = new Command(async () => await CompletarCita());
        CancelarCitaCommand = new Command(async () => await CancelarCitaSeleccionada());
        ActualizarEstadoCommand = new Command<EstadoCita>(async (estado) => await ActualizarEstadoCita(estado));

        CargarDatosSimulados();
    }

    private void CargarDatosSimulados()
    {
        var citasSimuladas = new List<Cita>
        {
            new Cita
            {
                Id = 1,
                FechaHora = DateTime.Today.AddHours(9),
                Motivo = "Revisión general",
                DuracionMinutos = 30,
                Estado = EstadoCita.Confirmada,
                Confirmada = true,
                FechaConfirmacion = DateTime.Now.AddDays(-1),
                Paciente = new Paciente { Id = 1, Nombre = "Carlos", Apellido = "López Martínez", Cedula = "12345678", Email = "carlos@email.com" }
            },
            new Cita
            {
                Id = 2,
                FechaHora = DateTime.Today.AddHours(10),
                Motivo = "Seguimiento cardiaco",
                DuracionMinutos = 45,
                Estado = EstadoCita.Pendiente,
                Confirmada = false,
                Paciente = new Paciente { Id = 2, Nombre = "María", Apellido = "González Rodríguez", Cedula = "87654321", Email = "maria@email.com" }
            },
            new Cita
            {
                Id = 3,
                FechaHora = DateTime.Today.AddHours(11),
                Motivo = "Electrocardiograma",
                DuracionMinutos = 20,
                Estado = EstadoCita.Confirmada,
                Confirmada = true,
                FechaConfirmacion = DateTime.Now.AddDays(-2),
                Paciente = new Paciente { Id = 3, Nombre = "Juan", Apellido = "Ramírez Santos", Cedula = "11111111", Email = "juan@email.com" }
            },
            new Cita
            {
                Id = 4,
                FechaHora = DateTime.Today.AddHours(14),
                Motivo = "Consulta general",
                DuracionMinutos = 30,
                Estado = EstadoCita.Pendiente,
                Confirmada = false,
                Paciente = new Paciente { Id = 4, Nombre = "Ana", Apellido = "Fernández López", Cedula = "22222222", Email = "ana@email.com" }
            },
            new Cita
            {
                Id = 5,
                FechaHora = DateTime.Today.AddDays(1).AddHours(9),
                Motivo = "Control de presión",
                DuracionMinutos = 30,
                Estado = EstadoCita.Confirmada,
                Confirmada = true,
                Paciente = new Paciente { Id = 5, Nombre = "Pedro", Apellido = "Diaz Gómez", Cedula = "33333333", Email = "pedro@email.com" }
            }
        };

        foreach (var cita in citasSimuladas)
        {
            Citas.Add(cita);
        }

        AplicarFiltros();
    }

    private async Task CargarCitas()
    {
        IsBusy = true;
        try
        {
            await Task.Delay(500); // Simular carga desde BD
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ConfirmarCita()
    {
        if (CitaSeleccionada == null) return;

        CitaSeleccionada.Estado = EstadoCita.Confirmada;
        CitaSeleccionada.Confirmada = true;
        CitaSeleccionada.FechaConfirmacion = DateTime.Now;
        CitaSeleccionada.FechaUltimaModificacion = DateTime.Now;

        await Application.Current!.MainPage!.DisplayAlert("Éxito", "Cita confirmada correctamente", "OK");
        AplicarFiltros();
    }

    private async Task CompletarCita()
    {
        if (CitaSeleccionada == null) return;

        CitaSeleccionada.Estado = EstadoCita.Completada;
        CitaSeleccionada.FechaUltimaModificacion = DateTime.Now;

        await Application.Current!.MainPage!.DisplayAlert("Éxito", "Cita marcada como completada", "OK");
        AplicarFiltros();
    }

    private async Task CancelarCitaSeleccionada()
    {
        if (CitaSeleccionada == null) return;

        bool confirmado = await Application.Current!.MainPage!.DisplayAlert(
            "Confirmar cancelación",
            $"¿Cancelar la cita de {CitaSeleccionada.Paciente?.NombreCompleto}?",
            "Sí",
            "No"
        );

        if (confirmado)
        {
            CitaSeleccionada.Estado = EstadoCita.Cancelada;
            CitaSeleccionada.FechaUltimaModificacion = DateTime.Now;
            await Application.Current!.MainPage!.DisplayAlert("Éxito", "Cita cancelada correctamente", "OK");
            AplicarFiltros();
        }
    }

    private async Task ActualizarEstadoCita(EstadoCita nuevoEstado)
    {
        if (CitaSeleccionada == null) return;

        CitaSeleccionada.Estado = nuevoEstado;
        CitaSeleccionada.FechaUltimaModificacion = DateTime.Now;

        await Task.Delay(300);
        AplicarFiltros();
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
}
