using System.Collections.ObjectModel;
using System.Windows.Input;
using DoctorApp.Models;
using DoctorApp.Services.Interfaces;
using DoctorApp.Security;
using DoctorApp.DTOs.Responses;

namespace DoctorApp.ViewModels;

/// <summary>
/// ViewModel para el Panel de Consultas del Dia
/// Carga el medico desde token/API y muestra consultas simuladas por ahora.
/// </summary>
public class PanelConsultasDelDiaViewModel : BaseViewModel
{
    private Medico _medicoActual = new();
    private ObservableCollection<PacienteConsultaModel> _consultasDelDia = new();
    private PacienteConsultaModel? _pacienteSeleccionado;
    private int _totalConsultas;
    private int _consultasCompletadas;
    private int _consultasEnCurso;
    private int _consultasEsperando;
    private EstadoConsulta _filtroEstado = EstadoConsulta.Esperando;
    private string _busquedaPaciente = string.Empty;

    // Servicios
    private readonly IDoctorService _doctorService;
    private readonly ITokenManager _tokenManager;

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

    public ICommand IniciarConsultaCommand { get; }
    public ICommand CompletarConsultaCommand { get; }
    public ICommand MarcarNoPrestoCommand { get; }
    public ICommand ReprogramarConsultaCommand { get; }
    public ICommand CargarConsultasCommand { get; }

    public PanelConsultasDelDiaViewModel(IDoctorService doctorService, ITokenManager tokenManager)
    {
        Title = "Panel de Consultas del Dia";
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));

        IniciarConsultaCommand = new Command(async () => await IniciarConsulta());
        CompletarConsultaCommand = new Command(async () => await CompletarConsulta());
        MarcarNoPrestoCommand = new Command(async () => await MarcarNoPresentado());
        ReprogramarConsultaCommand = new Command(async () => await ReprogramarConsulta());
        CargarConsultasCommand = new Command(async () => await CargarConsultasDelDia());

        _ = InicializarMedicoAsync();
        CargarDatosSimulados();
    }

    private async Task InicializarMedicoAsync()
    {
        var medicoId = await _tokenManager.GetUserIdAsync();
        var nombreToken = await _tokenManager.GetUserNameAsync();

        if (!string.IsNullOrWhiteSpace(nombreToken))
        {
            var (nombre, apellido) = SepararNombre(nombreToken);
            MedicoActual = new Medico
            {
                Id = medicoId ?? 0,
                Nombre = nombre,
                Apellido = apellido
            };
        }

        if (!medicoId.HasValue)
            return;

        try
        {
            _doctorService.EstablecerDoctorId(medicoId.Value);
            var doctorDto = await _doctorService.ObtenerDoctorActualAsync();
            MedicoActual = MapearDoctorDto(doctorDto);
        }
        catch
        {
            // Si falla, dejamos el nombre del token.
        }
    }

    private void CargarDatosSimulados()
    {
        var ahora = DateTime.Now;

        var consultasSimuladas = new List<PacienteConsultaModel>
        {
            new PacienteConsultaModel
            {
                Id = 1,
                Nombre = "Carlos",
                Apellido = "Lopez Martinez",
                Cedula = "12345678",
                Telefono = "+34 600 123 456",
                HoraConsulta = ahora.AddMinutes(5),
                Motivo = "Revision general",
                Estado = EstadoConsulta.Esperando,
                Edad = "45",
                AntecedentesRelevantes = "Hipertension, Diabetes tipo 2"
            },
            new PacienteConsultaModel
            {
                Id = 2,
                Nombre = "Maria",
                Apellido = "Gonzalez Rodriguez",
                Cedula = "87654321",
                Telefono = "+34 601 234 567",
                HoraConsulta = ahora.AddMinutes(35),
                Motivo = "Seguimiento cardiaco",
                Estado = EstadoConsulta.Esperando,
                Edad = "52",
                AntecedentesRelevantes = "Arritmia cardiaca"
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
            await Task.Delay(200);
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
                "Solo se puede iniciar consulta con pacientes en estado Esperando",
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
            "Exito",
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
            $"Marcar a {PacienteSeleccionado.NombreCompleto} como no presentado?",
            "Si",
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
            $"Consulta de {PacienteSeleccionado.NombreCompleto} marcada para reprogramacion",
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

    private static Medico MapearDoctorDto(DoctorResponseDto dto)
    {
        var (nombre, apellido) = SepararNombre(dto.Nombre);
        return new Medico
        {
            Id = dto.Id,
            Nombre = nombre,
            Apellido = apellido,
            Especialidad = dto.Especialidad ?? dto.NombreEspecialidad ?? string.Empty,
            Consultorio = dto.ProveedorSalud ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            Telefono = dto.TelefonoConsultorio ?? string.Empty,
            NumeroLicencia = dto.Exequatur ?? string.Empty,
            Activo = dto.Activo,
            FechaRegistro = DateTime.Now
        };
    }

    private static (string nombre, string apellido) SepararNombre(string nombreCompleto)
    {
        var limpio = (nombreCompleto ?? string.Empty).Trim();
        if (limpio.StartsWith("Dr. ", StringComparison.OrdinalIgnoreCase))
            limpio = limpio.Substring(4).Trim();
        if (limpio.StartsWith("Dra. ", StringComparison.OrdinalIgnoreCase))
            limpio = limpio.Substring(5).Trim();

        var partes = limpio.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0)
            return ("Medico", string.Empty);

        var nombre = partes[0];
        var apellido = partes.Length > 1 ? string.Join(" ", partes.Skip(1)) : string.Empty;
        return (nombre, apellido);
    }
}
