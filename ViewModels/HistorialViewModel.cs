using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using SemilleroGR3.Models;
using SemilleroGR3.Services;
using System.Collections.ObjectModel;

namespace SemilleroGR3.ViewModels
{
    public partial class HistorialViewModel : ObservableObject
    {
        private readonly FamiliaService _familiaService;
        private List<GrupoFecha> _todosPeriodos = new();
        private int _paginaActual = 0;
        private const int TamPagina = 4;

        [ObservableProperty] private ObservableCollection<GrupoFecha> periodos = new();
        [ObservableProperty] private ObservableCollection<BarraProgreso> tendencia = new();
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isLoadingMore;
        [ObservableProperty] private bool hayMas;
        [ObservableProperty] private bool hayError;
        [ObservableProperty] private string mensajeError = string.Empty;
        [ObservableProperty] private int totalEvaluaciones;
        [ObservableProperty] private string nivelGeneral = "Sin datos";
        [ObservableProperty] private string colorNivelGeneral = "#9E9E9E";

        public HistorialViewModel(FamiliaService familiaService)
        {
            _familiaService = familiaService;
            // Escucha el cambio de hijo igual que SeguimientoViewModel y TareasCasaViewModel
            WeakReferenceMessenger.Default.Register<CambiarHijoMessage>(this, (r, m) =>
            {
                _ = CargarHistorialAsync(m.NuevoId);
            });
        }

        // ── Carga principal ───────────────────────────────────────────────
        public async Task CargarHistorialAsync(int? alumnoId = null)
        {
            int id = alumnoId ?? Preferences.Get("HijoActivoId", 0);
            if (id <= 0) return;

            IsBusy = true;
            HayError = false;
            _paginaActual = 0;
            Periodos.Clear();
            Tendencia.Clear();
            _todosPeriodos.Clear();

            try
            {
                var evaluaciones = await _familiaService.GetEvaluacionesHijoAsync(id);

                if (evaluaciones == null || evaluaciones.Count == 0)
                {
                    HayError = true;
                    MensajeError = "No hay evaluaciones registradas para este alumno aún.";
                    return;
                }

                // Agrupar por fecha de evaluación
                _todosPeriodos = evaluaciones
                    .GroupBy(e => e.FechaEvaluacion.Date)
                    .OrderByDescending(g => g.Key)
                    .Select(g => new GrupoFecha
                    {
                        Fecha = g.Key,
                        FechaTexto = g.Key.ToString("dd 'de' MMMM yyyy",
                            new System.Globalization.CultureInfo("es-EC")),
                        Evaluaciones = g.ToList(),
                        PromedioNivel = g.SelectMany(e => e.Detalles ?? new())
                                         .Where(d => NivelANumero(d.NivelNombre) > 0)
                                         .Select(d => NivelANumero(d.NivelNombre))
                                         .DefaultIfEmpty(0)
                                         .Average()
                    })
                    .ToList();

                // Resumen global
                TotalEvaluaciones = evaluaciones.Count;
                double prom = _todosPeriodos.Any() ? _todosPeriodos.Average(p => p.PromedioNivel) : 0;
                NivelGeneral = NivelTexto(prom);
                ColorNivelGeneral = NivelColor(prom);

                // Gráfico de tendencia (últimos 6 períodos)
                foreach (var p in _todosPeriodos.OrderBy(x => x.Fecha).TakeLast(6))
                {
                    Tendencia.Add(new BarraProgreso
                    {
                        Etiqueta    = p.Fecha.ToString("dd/MM"),
                        Valor       = p.PromedioNivel,
                        ValorTexto  = p.PromedioNivel.ToString("F1"),
                        Altura      = Math.Max(8, (int)(p.PromedioNivel * 20)),
                        Color       = NivelColor(p.PromedioNivel),
                        NivelTexto  = NivelTexto(p.PromedioNivel),
                    });
                }

                // Primera página del listado
                CargarSiguientePagina();
            }
            catch (Exception ex)
            {
                HayError = true;
                MensajeError = "No se pudo conectar con el servidor. Verifica tu conexión.";
                System.Diagnostics.Debug.WriteLine($"[HistorialVM] {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        // ── Lazy loading (RNF-04) ─────────────────────────────────────────
        [RelayCommand]
        private async Task CargarMas()
        {
            if (IsLoadingMore || !HayMas) return;
            IsLoadingMore = true;
            await Task.Delay(200);
            CargarSiguientePagina();
            IsLoadingMore = false;
        }

        private void CargarSiguientePagina()
        {
            var pagina = _todosPeriodos
                .Skip(_paginaActual * TamPagina)
                .Take(TamPagina)
                .ToList();
            foreach (var g in pagina)
                Periodos.Add(g);
            _paginaActual++;
            HayMas = (_paginaActual * TamPagina) < _todosPeriodos.Count;
        }

        // ── Helpers estáticos (reutilizados por GrupoFecha) ──────────────
        public static int NivelANumero(string nivel) => nivel?.ToLower() switch
        {
            "iniciado"   => 1,
            "en proceso" => 2,
            "logrado"    => 3,
            "destacado"  => 4,
            _            => 0
        };

        public static string NivelTexto(double n) => n switch
        {
            >= 3.5 => "Destacado ⭐",
            >= 2.5 => "Logrado ✅",
            >= 1.5 => "En Proceso 🔄",
            > 0    => "Iniciado ⚠️",
            _      => "Sin datos"
        };

        public static string NivelColor(double n) => n switch
        {
            >= 3.5 => "#2E7D32",
            >= 2.5 => "#1565C0",
            >= 1.5 => "#E65100",
            > 0    => "#B71C1C",
            _      => "#9E9E9E"
        };
    }

    /// <summary>Agrupa las evaluaciones de un mismo día.</summary>
    public class GrupoFecha
    {
        public DateTime Fecha { get; set; }
        public string FechaTexto { get; set; } = string.Empty;
        public List<Evaluacion> Evaluaciones { get; set; } = new();
        public double PromedioNivel { get; set; }

        public string ColorPromedio   => HistorialViewModel.NivelColor(PromedioNivel);
        public string TextoPromedio   => HistorialViewModel.NivelTexto(PromedioNivel);
        public string PromedioFormateado => PromedioNivel.ToString("F1");
        public int TotalCriterios    => Evaluaciones.Sum(e => e.Detalles?.Count ?? 0);
    }

    /// <summary>Punto del gráfico de barras de tendencia.</summary>
    public class BarraProgreso
    {
        public string Etiqueta   { get; set; } = string.Empty;
        public double Valor      { get; set; }
        public string ValorTexto { get; set; } = string.Empty;
        public int    Altura     { get; set; }
        public string Color      { get; set; } = "#3949AB";
        public string NivelTexto { get; set; } = string.Empty;
    }
}
