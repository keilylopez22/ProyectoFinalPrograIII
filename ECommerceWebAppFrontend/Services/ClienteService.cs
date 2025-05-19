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

        //Se agrega un nuevo método para obtener todos los clientes en pedidos
        //Agregado por Chamo
        public async Task<List<Cliente>> ObtenerTodosLosClientesAsync()
        {
            try
            {
                var resultado = await ObtenerClientesAsync("", 1, 1000); //Traer todos los clientes
                return resultado?.Clientes ?? new List<Cliente>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }

        public async Task CrearClienteAsync(Cliente cliente) =>
            await http.PostAsJsonAsync("api/clientes", cliente);

        public async Task ModificarClienteAsync(Cliente cliente) =>
            await http.PutAsJsonAsync($"api/clientes/{cliente.Id}", cliente);

        public async Task EliminarClienteAsync(int id) =>
            await http.DeleteAsync($"api/clientes/{id}");
    }

}