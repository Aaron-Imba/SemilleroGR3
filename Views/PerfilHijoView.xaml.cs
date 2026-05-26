namespace SemilleroGR3.Views;

using SemilleroGR3.ViewModels;

public partial class PerfilHijoView : ContentPage
{
    public PerfilHijoView(PerfilHijoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}