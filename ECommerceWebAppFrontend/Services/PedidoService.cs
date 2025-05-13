using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiECommerce.Modelo; // Asegúrate de que aquí esté tu modelo Pedido

namespace ECommerceWebAppFrontend.Services
{
    public class PedidoService
    {
        private readonly HttpClient _http;

        public PedidoService(HttpClient http) => _http = http;

        public async Task<List<Pedido>> ObtenerPedidosAsync(string? cliente = null, int pageNumber = 1, int pageSize = 10)
        {
            string url = $"api/Pedido?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(cliente))
            {
                url += $"&cliente={cliente}";
            }

            var response = await _http.GetFromJsonAsync<List<Pedido>>(url);
            return response ?? new List<Pedido>();
        }

        public async Task CrearPedidoAsync(Pedido pedido) =>
            await _http.PostAsJsonAsync("api/Pedido", pedido);

        public async Task ModificarPedidoAsync(Pedido pedido)
        {
            Console.WriteLine($"Modificando pedido: {pedido.Id}");
            await _http.PutAsJsonAsync($"api/Pedido/{pedido.Id}", pedido);
        }

        public async Task EliminarPedidoAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/Pedido/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}