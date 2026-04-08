using DoctorApp.ViewModels;

namespace DoctorApp.Views;

public partial class PanelConsultasDelDiaPage : ContentPage
{
    public PanelConsultasDelDiaPage()
    {
        InitializeComponent();
        BindingContext = new PanelConsultasDelDiaViewModel();
    }
}
