namespace SemilleroGR3.Views;

using SemilleroGR3.ViewModels;

public partial class SeguimientoView : ContentPage
{
    public SeguimientoView(SeguimientoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}