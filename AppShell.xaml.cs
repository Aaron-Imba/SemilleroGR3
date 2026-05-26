using SemilleroGR3.Views;

namespace SemilleroGR3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas que no están directamente en el TabBar
            Routing.RegisterRoute(nameof(PerfilHijoView), typeof(PerfilHijoView));
        }
    }
}