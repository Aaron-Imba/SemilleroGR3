
using SemilleroGR3.ViewModels;


namespace SemilleroGR3.Views;

public partial class ProgresoView : ContentPage
{
	public ProgresoView(ProgresoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}