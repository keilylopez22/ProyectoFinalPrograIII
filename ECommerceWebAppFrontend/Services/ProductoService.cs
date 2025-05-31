using System.Net.Http;                   
using System.Net.Http.Json;                 
using System.Threading.Tasks;               
using System.Collections.Generic;
using ApiECommerce.DTOs;   
using ApiECommerce.Modelo;      
                 
namespace ECommerceWebAppFrontend.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        public ProductoService(HttpClient http) => _http = http;

        public async Task<ResultadoPaginadoProductoDTO> ObtenerProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 10)
        { 
            string url = $"api/Producto?pageNumber={pageNumber}&pageSize={pageSize}";

            if (categoriaId.HasValue)
            {
                url += $"&categoriaId={categoriaId.Value}";
            } 

            var response = await _http.GetFromJsonAsync<ResultadoPaginadoProductoDTO>(url);
            return response ?? new ResultadoPaginadoProductoDTO();
        }

        public async Task<ProductoDTO?> ObtenerProductoPorIdAsync(int id)
        {
            var response = await _http.GetFromJsonAsync<ProductoDTO>($"api/Producto/{id}");
            return response;
        }
        public async Task<List<ProductoDTO>> ObtenerListaProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 100)
        {
            string url = $"api/Producto?pageNumber={pageNumber}&pageSize={pageSize}";

            if (categoriaId.HasValue)
            {
                url += $"&categoriaId={categoriaId.Value}";
            }

            var response = await _http.GetFromJsonAsync<ResultadoPaginadoProductoDTO>(url);
            return response.Productos ?? new List<ProductoDTO>();

        }

               // Si necesitas obtener categorías, usa el DTO también
        public async Task<List<CategoriaDTO>> ObtenerCategoriasAsync()
        {
            var response = await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/Categorias");
            return response ?? new List<CategoriaDTO>();
        }

        // Métodos para crear, modificar y eliminar productos pueden seguir usando ProductoDTO
        public async Task CrearProductoAsync(ProductoDTO producto) =>
            await _http.PostAsJsonAsync("api/Producto", producto);

        public async Task ModificarProductoAsync(ProductoDTO producto)
        {
            await _http.PutAsJsonAsync($"api/Producto/{producto.Id}", producto);
        }

        public async Task EliminarProductoAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Producto/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}