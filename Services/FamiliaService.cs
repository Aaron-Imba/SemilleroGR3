using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SemilleroGR3.Models;

namespace SemilleroGR3.Services
{
    public class FamiliaService
    {
        private readonly ApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions;

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
    }
}
