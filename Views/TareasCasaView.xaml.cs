using SemilleroGR3.ViewModels;

namespace SemilleroGR3.Views
{
    public partial class TareasCasaView : ContentPage
    {
        public TareasCasaView(TareasCasaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}