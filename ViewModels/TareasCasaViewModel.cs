using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using SemilleroGR3.Models;
using SemilleroGR3.Services;


namespace SemilleroGR3.ViewModels
{
    public partial class TareasCasaViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<TareaCasa> tareas;

        public TareasCasaViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            Tareas = new ObservableCollection<TareaCasa>();

            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarTareasAsync(m.NuevoId);
            });
        }

        public async Task CargarTareasAsync(int? alumnoId = null)
        {
            int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);
            if (idBusqueda > 0)
            {
                var lista = await _familiaService.GetTareasCasaAsync(idBusqueda);
                Tareas.Clear();
                foreach (var tarea in lista)
                {
                    // Mostrar sólo tareas que no estén ya realizadas o reportadas
                    if (!tarea.Realizada && tarea.FechaReporte == null)
                    {
                        Tareas.Add(tarea);
                    }
                }
            }
        }

        [RelayCommand]
        public async Task MarcarComoRealizadaAsync(TareaCasa tarea)
        {
            if (tarea == null || tarea.Realizada) return;

            // Llama al API para actualizar el estado
            bool exito = await _familiaService.MarcarTareaRealizadaAsync(tarea.Id, tarea.ComentarioBreve);

            if (exito)
            {
                tarea.Realizada = true;
                tarea.FechaReporte = DateTime.Now;
                // Forzamos actualización visual (opcional dependiendo del Binding)
                OnPropertyChanged(nameof(Tareas));
            }
        }
    }
}
