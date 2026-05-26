using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SemilleroGR3.Models;
using SemilleroGR3.Services;
using System.Collections.ObjectModel;

namespace SemilleroGR3.ViewModels
{
    public partial class TareasCasaViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;

        [ObservableProperty]
        private ObservableCollection<TareaCasa> tareas;

        [ObservableProperty]
        private bool isBusy;

        public TareasCasaViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            Tareas = new ObservableCollection<TareaCasa>();

            // Escuchar si se cambia de hijo en el Dashboard
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarTareasAsync(m.NuevoId);
            });

            // Carga inicial automática al abrir la pestaña
            _ = CargarTareasAsync();
        }

        public async Task CargarTareasAsync(int? alumnoId = null)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                int idBusqueda = alumnoId ?? Preferences.Get("HijoActivoId", 0);

                if (idBusqueda > 0)
                {
                    var lista = await _familiaService.GetTareasCasaAsync(idBusqueda);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Tareas.Clear();
                        foreach (var tarea in lista)
                        {
                            Tareas.Add(tarea);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar tareas: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task MarcarComoRealizadaAsync(TareaCasa tarea)
        {
            // Evita dobles envíos
            if (tarea == null || tarea.Realizada || isBusy) return;

            try
            {
                IsBusy = true;

                // Si el padre no escribió nada, ponemos un comentario por defecto
                string comentario = string.IsNullOrWhiteSpace(tarea.ComentarioBreve)
                                    ? "Completada sin comentarios."
                                    : tarea.ComentarioBreve;

                // Llama al API para actualizar el estado
                bool exito = await _familiaService.MarcarTareaRealizadaAsync(tarea.Id, comentario);

                if (exito)
                {
                    // Recargamos la lista desde la API para asegurar que los datos estén sincronizados
                    // y que la UI se actualice automáticamente al estado "Completada"
                    await CargarTareasAsync(Preferences.Get("HijoActivoId", 0));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar la tarea: {ex.Message}");
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsBusy = false;
                });
            }
        }
    }
}