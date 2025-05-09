using System.Net.Http;                   
using System.Net.Http.Json;                 
using System.Threading.Tasks;               
using System.Collections.Generic;           
using ApiECommerce.Modelo;                  

namespace ECommerceWebAppFrontend.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        public ProductoService(HttpClient http) => _http = http;

        public async Task<List<Producto>> ObtenerProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 10)
        {
            string url = $"api/Producto?pageNumber={pageNumber}&pageSize={pageSize}";

            if (categoriaId.HasValue)
            {
                url += $"&categoriaId={categoriaId.Value}";
            }

            var response = await _http.GetFromJsonAsync<List<Producto>>(url);
            return response ?? new List<Producto>();
        }

        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            var response = await _http.GetFromJsonAsync<List<Categoria>>("api/Categorias");
            return response ?? new List<Categoria>();
        }   


        public async Task CrearProductoAsync(Producto producto) =>
            await _http.PostAsJsonAsync("api/Producto", producto);

        public async Task ModificarProductoAsync(Producto producto)
        {
            Console.WriteLine($"Modificando producto: {producto.Id}");
            await _http.PutAsJsonAsync($"api/Producto/{producto.Id}", producto);
        }

        public async Task EliminarProductoAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Producto/{id}");
            response.EnsureSuccessStatusCode();
        }

    }

}