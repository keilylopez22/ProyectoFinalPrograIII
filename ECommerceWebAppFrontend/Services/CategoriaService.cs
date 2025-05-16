using System.Net.Http.Json;
using ApiECommerce.DTOs;

namespace ECommerceWebAppFrontend.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoriaDTO>> ObtenerCategoriasAsync()
        {
            return await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/Categorias");
        }
    }
}