using DoctorApp.ViewModels;

namespace DoctorApp.Views;

public partial class GestionCitasPage : ContentPage
{
    public GestionCitasPage()
    {
        InitializeComponent();
        BindingContext = new GestionCitasViewModel();
    }
}
