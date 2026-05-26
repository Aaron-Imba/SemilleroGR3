namespace SemilleroGR3.Views;

using SemilleroGR3.ViewModels;

public partial class DashboardView : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarHijosAsync();
    }
}