using System.Net.Http;                   
using System.Net.Http.Json;                 
using System.Threading.Tasks;               
using System.Collections.Generic;           
using ApiECommerce.Modelo; 
using ApiECommerce.DTOs;
namespace ECommerceWebAppFrontend.Services
{
    /*public class ClienteService
    {
        
        private readonly HttpClient _http;

        public ClienteService(HttpClient http) => _http = http;

       public async Task<List<Cliente>> ObtenerClientesAsync(string? nombre = null, int pagina = 1, int porPagina = 10)
        {
            string url = $"api/Clientes?pagina={pagina}&porPagina={porPagina}";

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                url += $"&nombre={Uri.EscapeDataString(nombre)}";
            }

            var response = await _http.GetFromJsonAsync<List<Cliente>>(url);
            return response ?? new List<Cliente>();
        }

        public async Task CrearClienteAsync(Cliente cliente) =>
            await _http.PostAsJsonAsync("api/Clientes", cliente);

        public async Task ModificarClienteAsync(Cliente cliente)
        {
            Console.WriteLine($"Modificando cliente: {cliente.Id}");
            await _http.PutAsJsonAsync($"api/Clientes/{cliente.Id}", cliente);
        }
        public async Task EliminarClienteAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Clientes/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<Cliente> ObtenerClientePorIdAsync(int id)
        {
            var response = await _http.GetFromJsonAsync<Cliente>($"api/Clientes/{id}");
            return response;
        }
        
    }*/
    public class ClienteService
    {
        private readonly HttpClient http;

        public ClienteService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<ResultadoClientes> ObtenerClientesAsync(string filtro, int pagina, int porPagina)
        {
            var response = await http.GetFromJsonAsync<ResultadoClientes>($"api/clientes?nombre={filtro}&pageNumber={pagina}&pageSize={porPagina}");
            return response ?? new ResultadoClientes();
        }

        public async Task CrearClienteAsync(Cliente cliente) =>
            await http.PostAsJsonAsync("api/clientes", cliente);

        public async Task ModificarClienteAsync(Cliente cliente) =>
            await http.PutAsJsonAsync($"api/clientes/{cliente.Id}", cliente);

        public async Task EliminarClienteAsync(int id) =>
            await http.DeleteAsync($"api/clientes/{id}");
    }

}