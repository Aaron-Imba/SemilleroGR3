using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace SemilleroGR3.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        // URL base apuntando a tu API local desde el emulador de Android
        private readonly string _baseUrl = "https://10.0.2.2:7019/api/";

        public ApiClient()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30) // RNF-04: Evitar bloqueos infinitos
            };
        }

        // Método clave que inyecta el JWT antes de hacer cualquier petición
        public async Task<HttpClient> GetClientAsync()
        {
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }

            return _httpClient;
        }
    }
}