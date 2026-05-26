using SemilleroGR3.Models;
using SemilleroGR3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;

namespace SemilleroGR3.ViewModels
{
    public partial class SeguimientoViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<Evaluacion> evaluaciones;

        [ObservableProperty]
        private bool isBusy;

        public SeguimientoViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            Evaluaciones = new ObservableCollection<Evaluacion>();

            // Escuchar si el padre cambia de hijo en el Dashboard
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarSeguimientoAsync(m.NuevoId);
            });
        }

        public async Task CargarSeguimientoAsync(int? alumnoId = null)
        {
            IsBusy = true;
            int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);

            if (idBusqueda > 0)
            {
                var lista = await _familiaService.GetEvaluacionesHijoAsync(idBusqueda);
                Evaluaciones.Clear();
                foreach (var eval in lista)
                {
                    Evaluaciones.Add(eval);
                }
            }
            IsBusy = false;
        }
    }
}
