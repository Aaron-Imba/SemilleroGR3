using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SemilleroGR3.Models;
using Microsoft.Maui.Networking; // Añadido para verificar conexión a internet
using Microsoft.Maui.Storage;    // Añadido para guardar caché offline
using SemilleroGR3.Helpers;

namespace SemilleroGR3.Services
{
    public class FamiliaService
    {
        private readonly ApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private static List<CriterioEvaluacionDto> _cacheProgreso;

        public FamiliaService(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // RF-F08: Obtener la lista de hijos asociados a este perfil de familia
        public async Task<List<Alumno>> GetHijosAsync(int familiaId)
        {
            var client = await _apiClient.GetClientAsync();
            var response = await client.GetAsync($"familia/{familiaId}/hijos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Alumno>>(content, _jsonOptions);
            }
            return new List<Alumno>();
        }

        // RF-F02 y RF-F03: Obtener rúbricas de un hijo
        public async Task<List<Evaluacion>> GetEvaluacionesHijoAsync(int alumnoId)
        {
            var client = await _apiClient.GetClientAsync();
            var response = await client.GetAsync($"evaluaciones/alumno/{alumnoId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Evaluacion>>(content, _jsonOptions);
            }
            return new List<Evaluacion>();
        }

        // RF-F04: Obtener tareas asignadas a casa
        public async Task<List<TareaCasa>> GetTareasCasaAsync(int alumnoId)
        {
            var client = await _apiClient.GetClientAsync();
            var response = await client.GetAsync($"tareas/alumno/{alumnoId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TareaCasa>>(content, _jsonOptions);
            }
            return new List<TareaCasa>();
        }

        // RF-F04: Actualizar estado de la tarea (Enviar que ya se realizó)
        public async Task<bool> MarcarTareaRealizadaAsync(int tareaId, string comentario)
        {
            var client = await _apiClient.GetClientAsync();
            var updateData = new { Realizada = true, ComentarioBreve = comentario, FechaReporte = DateTime.UtcNow };
            var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"tareas/{tareaId}/estado", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CriterioEvaluacionDto>> GetProgresoHijoAsync(int alumnoId)
        {
            // 1. Validar conexión a internet
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var client = await _apiClient.GetClientAsync();

                    // Llama al endpoint de tu FamiliaController en .NET 9
                    var response = await client.GetAsync($"familia/progreso/{alumnoId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var resultado = JsonSerializer.Deserialize<List<CriterioEvaluacionDto>>(content, _jsonOptions);

                        if (resultado != null)
                        {
                            // Sincronizar Caché: Guardamos en memoria RAM y en disco local (Preferences)
                            _cacheProgreso = resultado;
                            Preferences.Default.Set($"{Constants.CacheEvaluacionesKey}{alumnoId}", content);
                            return resultado;
                        }
                    }
                }
                catch
                {
                    // Tolerancia a fallos: Si la API local se cae, lee los datos locales
                    return ObtenerProgresoDeCache(alumnoId);
                }
            }

            // 2. Sin internet: Carga inmediata desde el almacenamiento offline
            return ObtenerProgresoDeCache(alumnoId);
        }

        // Método auxiliar para extraer la caché de forma segura
        private List<CriterioEvaluacionDto> ObtenerProgresoDeCache(int alumnoId)
        {
            if (_cacheProgreso != null) return _cacheProgreso;

            string claveCache = $"{Constants.CacheEvaluacionesKey}{alumnoId}";
            if (Preferences.Default.ContainsKey(claveCache))
            {
                string jsonString = Preferences.Default.Get(claveCache, string.Empty);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonSerializer.Deserialize<List<CriterioEvaluacionDto>>(jsonString, _jsonOptions) ?? new List<CriterioEvaluacionDto>();
                }
            }

            return new List<CriterioEvaluacionDto>();
        }
    }
}

