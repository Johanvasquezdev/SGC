using DoctorApp.ViewModels;

namespace DoctorApp.Views;

public partial class GestionCitasPage : ContentPage
{
    public GestionCitasPage()
    {
        InitializeComponent();
        var citasService = Application.Current!.Handler.MauiContext!.Services.GetRequiredService<DoctorApp.Services.Interfaces.ICitasService>();
        BindingContext = new GestionCitasViewModel(citasService);
    }
}
