using DoctorApp.ViewModels;
using DoctorApp.Services.Interfaces;
using DoctorApp.Services.Hubs;

namespace DoctorApp.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();

        // Obtener servicios del contenedor DI
        var citasService = Application.Current!.Handler.MauiContext!.Services.GetRequiredService<ICitasService>();
        var doctorService = Application.Current!.Handler.MauiContext!.Services.GetRequiredService<IDoctorService>();
        var citasHubClient = Application.Current!.Handler.MauiContext!.Services.GetRequiredService<ICitasHubClient>();

        // Inyectar servicios en el ViewModel
        BindingContext = new DashboardViewModel(citasService, doctorService, citasHubClient);
    }
}

