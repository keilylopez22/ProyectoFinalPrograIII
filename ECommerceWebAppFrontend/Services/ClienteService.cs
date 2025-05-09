using System.Net.Http;                   
using System.Net.Http.Json;                 
using System.Threading.Tasks;               
using System.Collections.Generic;           
using ApiECommerce.Modelo; 
using ApiECommerce.DTOs;

namespace ECommerceWebAppFrontend.Services
{
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