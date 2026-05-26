using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using SemilleroGR3.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SemilleroGR3.ViewModels
{
    public partial class PerfilHijoViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private string nombreCompleto;

        [ObservableProperty]
        private string fechaNacimientoTexto;

        [ObservableProperty]
        private string grupoAsignado;

        [ObservableProperty]
        private string docenteResponsable;

        [ObservableProperty]
        private bool isBusy;

        public PerfilHijoViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;

            // Escucha de forma reactiva el cambio de hijo desde el Dashboard
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarPerfilHijoAsync(m.NuevoId);
            });
        }

        public async Task CargarPerfilHijoAsync(int? alumnoId = null)
        {
            int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);
            if (idBusqueda <= 0) return;

            IsBusy = true;

            try
            {
                // Reutilizamos el servicio. El backend debe retornar los datos del alumno 
                // junto con los JOINs de Grupo y Docente mapeados directamente.
                var alumnos = await _familiaService.GetHijosAsync(int.Parse(SecureStorage.Default.GetAsync("usuario_id").Result ?? "0"));
                var hijoActual = alumnos.Find(a => a.Id == idBusqueda);

                if (hijoActual != null)
                {
                    NombreCompleto = hijoActual.NombreCompleto;
                    FechaNacimientoTexto = hijoActual.FechaNacimiento.ToString("dd/MM/yyyy");

                    // NOTA: Estas propiedades dinámicas asumimos que tu API las incluirá en la respuesta 
                    // extendida de la consulta de alumnos o mediante un endpoint específico de perfil.
                    GrupoAsignado = "Grupo Inicial 2 - Sección A"; // Ejemplo de mapeo plano
                    DocenteResponsable = "Lic. Nathaly Romero";      // Ejemplo de mapeo plano
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar perfil: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
