using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using SemilleroGR3.Models;
using SemilleroGR3.Services;

namespace SemilleroGR3.ViewModels
{
    public partial class ProgresoViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<CriterioEvaluacionDto> criterios;

        [ObservableProperty]
        private bool isBusy;

        // 1. Inyectamos el servicio en el constructor
        public ProgresoViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            Criterios = new ObservableCollection<CriterioEvaluacionDto>();

            // 2. Escuchamos si el padre cambia de hijo en el Dashboard
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarProgresoAsync(m.NuevoId);
            });

            // 3. Hacemos una carga inicial automática
            _ = CargarProgresoAsync();
        }

        public async Task CargarProgresoAsync(int? alumnoId = null)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                // Leemos el ID que pasaron por parámetro o el que guardó el Dashboard
                int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);

                if (idBusqueda > 0)
                {
                    // Llamamos a la API real a través de tu servicio
                    var lista = await _familiaService.GetProgresoHijoAsync(idBusqueda);

                    // Actualizamos la interfaz gráfica de forma segura en el hilo principal
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Criterios.Clear();
                        foreach (var item in lista)
                        {
                            Criterios.Add(item);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Si la API falla o no hay internet, podemos verlo en la consola
                System.Diagnostics.Debug.WriteLine($"Error al cargar el progreso cognitivo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}