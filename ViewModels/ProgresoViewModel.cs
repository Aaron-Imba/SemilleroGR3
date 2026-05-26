
using CommunityToolkit.Mvvm.ComponentModel;
using SemilleroGR3.Models;
using System.Collections.ObjectModel;

namespace SemilleroGR3.ViewModels
{
    // Heredar de ObservableObject nos da la magia de MVVM sin código extra
    public partial class ProgresoViewModel : ObservableObject
    {
        // Esta colección guardará la lista de criterios que mostraremos en la pantalla
        [ObservableProperty]
        private ObservableCollection<CriterioEvaluacionDto> criterios;

        [ObservableProperty]
        private bool isBusy;

        public ProgresoViewModel()
        {
            // Inicializamos la colección vacía
            Criterios = new ObservableCollection<CriterioEvaluacionDto>();

            // Para poder probar la interfaz antes de tener la API,
            // podemos cargar datos falsos (mock data) temporalmente.
            CargarDatosDePrueba();
        }

        private void CargarDatosDePrueba()
        {
            IsBusy = true;

            // Simulamos lo que llegaría de SQL Server (Tablas CriterioCognitivo y NivelLogro)
            Criterios.Add(new CriterioEvaluacionDto
            {
                CriterioId = 1,
                NombreCriterio = "Clasificación",
                DescripcionCriterio = "Agrupa objetos por igualdad y semejanza.",
                CodigoLogro = "L", // Logrado (Se pintará verde)
                Observacion = "Logra clasificar personas seguras y peligrosas con facilidad."
            });

            Criterios.Add(new CriterioEvaluacionDto
            {
                CriterioId = 2,
                NombreCriterio = "Seriación",
                DescripcionCriterio = "Identifica el orden lógico de los eventos.",
                CodigoLogro = "EP", // En Proceso (Se pintará amarillo)
                Observacion = "Organiza secuencias simples pero se confunde con más de 4 pasos."
            });

            Criterios.Add(new CriterioEvaluacionDto
            {
                CriterioId = 3,
                NombreCriterio = "Pensamiento Lógico",
                DescripcionCriterio = "Justifica sus decisiones.",
                CodigoLogro = "EP", // Iniciado (Se pintará rojo/rosa)
                Observacion = "Aún no logra explicar por qué una acción es segura."
            });

            IsBusy = false;
        }
    }
}