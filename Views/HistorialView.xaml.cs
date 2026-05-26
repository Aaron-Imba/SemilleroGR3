namespace SemilleroGR3.Views;

using SemilleroGR3.ViewModels;

public partial class HistorialView : ContentPage
{
    public HistorialView(HistorialViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}