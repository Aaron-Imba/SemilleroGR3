using SemilleroGR3.Models;
using SemilleroGR3.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging; // 👈 ¡ESTA LÍNEA CORRIGE EL ERROR CS0103!
using Microsoft.Maui.Storage;

namespace SemilleroGR3.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<Alumno> hijos;

        [ObservableProperty]
        private Alumno hijoSeleccionado;

        public DashboardViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            Hijos = new ObservableCollection<Alumno>();
        }

        //public async Task CargarHijosAsync()
        //{
        //    // Asumimos que guardamos el FamiliaId al hacer login
        //    var familiaIdStr = await SecureStorage.Default.GetAsync("usuario_id");
        //    if (int.TryParse(familiaIdStr, out int familiaId))
        //    {
        //        var lista = await _familiaService.GetHijosAsync(familiaId);
        //        Hijos.Clear();
        //        foreach (var hijo in lista)
        //        {
        //            Hijos.Add(hijo);
        //        }

        //        if (Hijos.Count > 0)
        //        {
        //            HijoSeleccionado = Hijos[0]; // Selecciona el primero por defecto
        //        }
        //    }
        //}

        public async Task CargarHijosAsync()
        {
            // Asumimos que guardamos el FamiliaId al hacer login
            var familiaIdStr = await SecureStorage.Default.GetAsync("usuario_id");
            if (int.TryParse(familiaIdStr, out int familiaId))
            {
                var lista = await _familiaService.GetHijosAsync(familiaId);
                Hijos.Clear();
                foreach (var hijo in lista)
                {
                    Hijos.Add(hijo);
                }

                if (Hijos.Count > 0)
                {
                    // 1. Leemos el último ID que se guardó
                    int idGuardado = Preferences.Get("HijoActivoId", 0);

                    // 2. Buscamos si ese ID corresponde a algún hijo de la lista actual
                    var hijoPrevio = Hijos.FirstOrDefault(h => h.Id == idGuardado);

                    // 3. Si lo encontramos, lo seleccionamos. Si no hay nada guardado (o es la primera vez), seleccionamos el primero.
                    HijoSeleccionado = hijoPrevio ?? Hijos[0];
                }
            }
        }


        // Método que se dispara automáticamente cuando cambia 'HijoSeleccionado'
        partial void OnHijoSeleccionadoChanged(Alumno value)
        {
            if (value != null)
            {
                // Guardamos el ID del hijo activo para que Seguimiento y Tareas lo lean
                Preferences.Set("HijoActivoId", value.Id);

                // Notificación global para refrescar las otras pestañas
                WeakReferenceMessenger.Default.Send(new CambiarHijoMessage(value.Id));
            }
        }
    }

    // Mensaje simple para notificar a otras vistas
    public class CambiarHijoMessage
    {
        public int NuevoId { get; }
        public CambiarHijoMessage(int nuevoId)
        {
            NuevoId = nuevoId;
        }
    }
}