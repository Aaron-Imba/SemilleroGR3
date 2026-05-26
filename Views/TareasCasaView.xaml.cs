using SemilleroGR3.ViewModels;
using SemilleroGR3.Models;

namespace SemilleroGR3.Views
{
    public partial class TareasCasaView : ContentPage
    {
        public TareasCasaView(TareasCasaViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnEnviarReporteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is TareaCasa tarea)
            {
                bool confirmar = await DisplayAlert("Confirmar envío", "¿Deseas enviar el reporte para esta tarea?", "Sí", "No");
                if (!confirmar) return;

                if (BindingContext is TareasCasaViewModel vm)
                {
                    // Llamar al comando/ método asíncrono del ViewModel
                    await vm.MarcarComoRealizadaAsync(tarea);
                    // Opcional: mostrar confirmación visual
                    await DisplayAlert("Enviado", "El reporte se ha enviado correctamente.", "Aceptar");
                }
            }
        }
    }
}
