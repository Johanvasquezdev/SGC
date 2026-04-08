using System.Windows.Input;
using DoctorApp.Services.Interfaces;
using DoctorApp.Exceptions;

namespace DoctorApp.ViewModels;

/// <summary>
/// ViewModel para la página de login
/// Maneja la autenticación del usuario
/// </summary>
public class LoginViewModel : BaseViewModel
{
    private string _usuario = string.Empty;
    private string _contrasena = string.Empty;
    private string _mensajeEstado = string.Empty;
    private bool _mostrarMensaje;

    public string Usuario
    {
        get => _usuario;
        set
        {
            if (_usuario != value)
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }
    }

    public string Contrasena
    {
        get => _contrasena;
        set
        {
            if (_contrasena != value)
            {
                _contrasena = value;
                OnPropertyChanged();
            }
        }
    }

    public string MensajeEstado
    {
        get => _mensajeEstado;
        set
        {
            if (_mensajeEstado != value)
            {
                _mensajeEstado = value;
                OnPropertyChanged();
            }
        }
    }

    public bool MostrarMensaje
    {
        get => _mostrarMensaje;
        set
        {
            if (_mostrarMensaje != value)
            {
                _mostrarMensaje = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand LoginCommand { get; }

    private readonly IAuthService _authService;

    public LoginViewModel(IAuthService authService)
    {
        Title = "Iniciar Sesión";
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        LoginCommand = new Command(async () => await RealizarLogin());
    }

    /// <summary>
    /// Realiza el login del usuario
    /// </summary>
    private async Task RealizarLogin()
    {
        // Validar campos vacíos
        if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            MensajeEstado = "Usuario y contraseña son requeridos";
            MostrarMensaje = true;
            return;
        }

        IsBusy = true;
        MostrarMensaje = false;

        try
        {
            // Intentar login
            var respuesta = await _authService.LoginAsync(Usuario, Contrasena);

            if (respuesta != null && !string.IsNullOrEmpty(respuesta.AccessToken))
            {
                // Login exitoso, navegar al dashboard
                await Shell.Current.GoToAsync("DashboardPage");

                // Limpiar campos
                Usuario = string.Empty;
                Contrasena = string.Empty;
            }
            else
            {
                MensajeEstado = "Credenciales inválidas";
                MostrarMensaje = true;
            }
        }
        catch (UnauthorizedException)
        {
            MensajeEstado = "Usuario o contraseña incorrectos";
            MostrarMensaje = true;
        }
        catch (ConnectionException ex)
        {
            MensajeEstado = $"Error de conexión: {ex.Message}";
            MostrarMensaje = true;
        }
        catch (Exception ex)
        {
            MensajeEstado = $"Error: {ex.Message}";
            MostrarMensaje = true;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
