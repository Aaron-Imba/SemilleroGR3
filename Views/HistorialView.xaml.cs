using SemilleroGR3.ViewModels;

namespace SemilleroGR3.Views;

public partial class HistorialView : ContentPage
{
    private readonly HistorialViewModel _vm;

    public HistorialView(HistorialViewModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = viewModel;
    }

    // Se dispara cada vez que la pestaña se muestra
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Solo carga si aún no hay datos (evita recargar al volver de otra pestaña)
        if (!_vm.Periodos.Any())
            await _vm.CargarHistorialAsync();
    }
}
