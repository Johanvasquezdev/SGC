using System.Collections.ObjectModel;
using System.Windows.Input;
using DoctorApp.Models;

namespace DoctorApp.ViewModels;

/// <summary>
/// ViewModel para el Panel de Consultas del Día
/// Maneja la lógica de presentación para mostrar pacientes citados hoy
/// </summary>
public class PanelConsultasDelDiaViewModel : BaseViewModel
{
    private Medico _medicoActual;
    private ObservableCollection<PacienteConsultaModel> _consultasDelDia = new();
    private PacienteConsultaModel? _pacienteSeleccionado;
    private int _totalConsultas;
    private int _consultasCompletadas;
    private int _consultasEnCurso;
    private int _consultasEsperando;
    private EstadoConsulta _filtroEstado = EstadoConsulta.Esperando;
    private string _busquedaPaciente = string.Empty;

    // Propiedades Filtradas
    public ObservableCollection<PacienteConsultaModel> ConsultasFiltradas
    {
        get
        {
            var filtradas = Consultasdeldia.Where(c =>
                (FiltroEstado == EstadoConsulta.Esperando || c.Estado == FiltroEstado) &&
                (string.IsNullOrEmpty(BusquedaPaciente) ||
                 c.NombreCompleto.Contains(BusquedaPaciente, StringComparison.OrdinalIgnoreCase) ||
                 c.Cedula.Contains(BusquedaPaciente))
            ).OrderBy(c => c.HoraConsulta).ToList();

            var result = new ObservableCollection<PacienteConsultaModel>();
            foreach (var item in filtradas)
                result.Add(item);
            return result;
        }
    }

    // Propiedades

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

    public ObservableCollection<PacienteConsultaModel> Consultasdeldia
    {
        get => _consultasDelDia;
        set
        {
            if (_consultasDelDia != value)
            {
                _consultasDelDia = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConsultasFiltradas));
            }
        }
    }

    public PacienteConsultaModel? PacienteSeleccionado
    {
        get => _pacienteSeleccionado;
        set
        {
            if (_pacienteSeleccionado != value)
            {
                _pacienteSeleccionado = value;
                OnPropertyChanged();
            }
        }
    }

    public int TotalConsultas
    {
        get => _totalConsultas;
        set
        {
            if (_totalConsultas != value)
            {
                _totalConsultas = value;
                OnPropertyChanged();
            }
        }
    }

    public int ConsultasCompletadas
    {
        get => _consultasCompletadas;
        set
        {
            if (_consultasCompletadas != value)
            {
                _consultasCompletadas = value;
                OnPropertyChanged();
            }
        }
    }

    public int ConsultasEnCurso
    {
        get => _consultasEnCurso;
        set
        {
            if (_consultasEnCurso != value)
            {
                _consultasEnCurso = value;
                OnPropertyChanged();
            }
        }
    }

    public int ConsultasEsperando
    {
        get => _consultasEsperando;
        set
        {
            if (_consultasEsperando != value)
            {
                _consultasEsperando = value;
                OnPropertyChanged();
            }
        }
    }

    public EstadoConsulta FiltroEstado
    {
        get => _filtroEstado;
        set
        {
            if (_filtroEstado != value)
            {
                _filtroEstado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConsultasFiltradas));
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
                OnPropertyChanged(nameof(ConsultasFiltradas));
            }
        }
    }

    // Commands

    public ICommand IniciarConsultaCommand { get; }
    public ICommand CompletarConsultaCommand { get; }
    public ICommand MarcarNoPrestoCommand { get; }
    public ICommand ReprogramarConsultaCommand { get; }
    public ICommand CargarConsultasCommand { get; }

    // Constructor

    public PanelConsultasDelDiaViewModel()
    {
        Title = "Panel de Consultas del Día";

        // Inicializar Commands
        IniciarConsultaCommand = new Command(async () => await IniciarConsulta());
        CompletarConsultaCommand = new Command(async () => await CompletarConsulta());
        MarcarNoPrestoCommand = new Command(async () => await MarcarNoPresentado());
        ReprogramarConsultaCommand = new Command(async () => await ReprogramarConsulta());
        CargarConsultasCommand = new Command(async () => await CargarConsultasDelDia());

        // Inicializar médico
        MedicoActual = new Medico
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Pérez García",
            Especialidad = "Cardiología",
            Consultorio = "201"
        };

        // Cargar datos simulados
        CargarDatosSimulados();
    }

    // Métodos

    private void CargarDatosSimulados()
    {
        var ahora = DateTime.Now;

        var consultasSimuladas = new List<PacienteConsultaModel>
        {
            new PacienteConsultaModel
            {
                Id = 1,
                Nombre = "Carlos",
                Apellido = "López Martínez",
                Cedula = "12345678",
                Telefono = "+34 600 123 456",
                HoraConsulta = ahora.AddMinutes(5),
                Motivo = "Revisión general",
                Estado = EstadoConsulta.Esperando,
                Edad = "45",
                AntecedentesRelevantes = "Hipertensión, Diabetes tipo 2"
            },
            new PacienteConsultaModel
            {
                Id = 2,
                Nombre = "María",
                Apellido = "González Rodríguez",
                Cedula = "87654321",
                Telefono = "+34 601 234 567",
                HoraConsulta = ahora.AddMinutes(35),
                Motivo = "Seguimiento cardiaco",
                Estado = EstadoConsulta.Esperando,
                Edad = "52",
                AntecedentesRelevantes = "Arritmia cardíaca"
            },
            new PacienteConsultaModel
            {
                Id = 3,
                Nombre = "Juan",
                Apellido = "Ramírez Santos",
                Cedula = "11111111",
                Telefono = "+34 602 345 678",
                HoraConsulta = ahora.AddMinutes(65),
                Motivo = "Electrocardiograma",
                Estado = EstadoConsulta.Esperando,
                Edad = "38",
                AntecedentesRelevantes = "Sin antecedentes relevantes"
            },
            new PacienteConsultaModel
            {
                Id = 4,
                Nombre = "Ana",
                Apellido = "Fernández López",
                Cedula = "22222222",
                Telefono = "+34 603 456 789",
                HoraConsulta = ahora.AddMinutes(-15),
                Motivo = "Consulta general",
                Estado = EstadoConsulta.EnConsulta,
                Edad = "41",
                AntecedentesRelevantes = "Alergia a penicilina"
            },
            new PacienteConsultaModel
            {
                Id = 5,
                Nombre = "Pedro",
                Apellido = "Diaz Gómez",
                Cedula = "33333333",
                Telefono = "+34 604 567 890",
                HoraConsulta = ahora.AddMinutes(-45),
                Motivo = "Control de presión",
                Estado = EstadoConsulta.Completada,
                Edad = "58",
                AntecedentesRelevantes = "Colesterol elevado"
            }
        };

        foreach (var consulta in consultasSimuladas)
            Consultasdeldia.Add(consulta);

        ActualizarEstadisticas();
    }

    private async Task CargarConsultasDelDia()
    {
        IsBusy = true;
        try
        {
            // Aquí iría la llamada a la API
            await Task.Delay(500); // Simulamos carga
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task IniciarConsulta()
    {
        if (PacienteSeleccionado == null) return;

        if (PacienteSeleccionado.Estado != EstadoConsulta.Esperando)
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Aviso",
                "Solo se puede iniciar consulta con pacientes en estado 'Esperando'",
                "OK"
            );
            return;
        }

        PacienteSeleccionado.Estado = EstadoConsulta.EnConsulta;
        PacienteSeleccionado.FechaCreacion = DateTime.Now;

        await Application.Current!.MainPage!.DisplayAlert(
            "Consulta Iniciada",
            $"Iniciada consulta con {PacienteSeleccionado.NombreCompleto}",
            "OK"
        );

        ActualizarEstadisticas();
    }

    private async Task CompletarConsulta()
    {
        if (PacienteSeleccionado == null) return;

        PacienteSeleccionado.Estado = EstadoConsulta.Completada;

        await Application.Current!.MainPage!.DisplayAlert(
            "Éxito",
            $"Consulta completada para {PacienteSeleccionado.NombreCompleto}",
            "OK"
        );

        ActualizarEstadisticas();
    }

    private async Task MarcarNoPresentado()
    {
        if (PacienteSeleccionado == null) return;

        bool confirmado = await Application.Current!.MainPage!.DisplayAlert(
            "Confirmar",
            $"¿Marcar a {PacienteSeleccionado.NombreCompleto} como no presentado?",
            "Sí",
            "No"
        );

        if (confirmado)
        {
            PacienteSeleccionado.Estado = EstadoConsulta.NoPresento;
            await Application.Current!.MainPage!.DisplayAlert(
                "Registrado",
                "Paciente marcado como no presentado",
                "OK"
            );
            ActualizarEstadisticas();
        }
    }

    private async Task ReprogramarConsulta()
    {
        if (PacienteSeleccionado == null) return;

        PacienteSeleccionado.Estado = EstadoConsulta.Reprogramada;

        await Application.Current!.MainPage!.DisplayAlert(
            "Reprogramada",
            $"Consulta de {PacienteSeleccionado.NombreCompleto} marcada para reprogramación",
            "OK"
        );

        ActualizarEstadisticas();
    }

    private void ActualizarEstadisticas()
    {
        TotalConsultas = Consultasdeldia.Count;
        ConsultasCompletadas = Consultasdeldia.Count(c => c.Estado == EstadoConsulta.Completada);
        ConsultasEnCurso = Consultasdeldia.Count(c => c.Estado == EstadoConsulta.EnConsulta);
        ConsultasEsperando = Consultasdeldia.Count(c => c.Estado == EstadoConsulta.Esperando);

        OnPropertyChanged(nameof(ConsultasFiltradas));
    }
}
