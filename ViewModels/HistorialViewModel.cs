using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using SemilleroGR3.Models;
using SemilleroGR3.Services;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SemilleroGR3.ViewModels
{
    public partial class HistorialViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<Evaluacion> historialEvaluaciones;

        [ObservableProperty]
        private bool isBusy;

        public HistorialViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            HistorialEvaluaciones = new ObservableCollection<Evaluacion>();

            // Escucha el cambio global de hijo para refrescar el histórico cronológico
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarHistorialAsync(m.NuevoId);
            });
        }

        public async Task CargarHistorialAsync(int? alumnoId = null)
        {
            int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);
            if (idBusqueda <= 0) return;

            IsBusy = true;

            try
            {
                // Obtenemos el registro histórico completo de evaluaciones asociadas al alumno
                var lista = await _familiaService.GetEvaluacionesHijoAsync(idBusqueda);

                HistorialEvaluaciones.Clear();
                foreach (var evaluacion in lista)
                {
                    HistorialEvaluaciones.Add(evaluacion);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar historial: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
