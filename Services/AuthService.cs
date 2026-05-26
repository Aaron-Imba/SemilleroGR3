using SemilleroGR3.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace SemilleroGR3.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            try
            {
                var client = await _apiClient.GetClientAsync();

                // Construimos el cuerpo de la petición
                var loginData = new { Email = email, Password = password };
                var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

                // Petición POST al endpoint de autenticación
                var response = await client.PostAsync("auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var usuario = JsonSerializer.Deserialize<Usuario>(jsonResult, _jsonOptions);

                    // ¡Magia! Guardamos el JWT de forma segura en el dispositivo (RNF-01)
                    if (!string.IsNullOrEmpty(usuario?.Token))
                    {
                        await SecureStorage.Default.SetAsync("jwt_token", usuario.Token);
                        // Opcional: Guardar el ID de usuario o FamiliaId para uso rápido
                        await SecureStorage.Default.SetAsync("usuario_id", usuario.Id.ToString());
                    }

                    return usuario;
                }
                return null; // Login fallido (credenciales incorrectas)
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error
                Console.WriteLine($"Error en Login: {ex.Message}");
                return null;
            }
        }

        public void Logout()
        {
            // Limpiamos la sesión borrando los datos seguros
            SecureStorage.Default.Remove("jwt_token");
            SecureStorage.Default.Remove("usuario_id");
        }
    }
}
