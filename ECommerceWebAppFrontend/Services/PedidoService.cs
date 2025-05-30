using ApiECommerce.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiECommerce.Modelo;

namespace ECommerceWebAppFrontend.Services
{
    public class PedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient http)
        {
            _httpClient = http;
        }

         public async Task<ResultadoPedidos> ObtenerPedidosPaginadosAsync(
            int? idCliente = null,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int paginaActual = 1,
            int tamanioPagina = 10)
        {
            try
            {
                var queryParams = new List<string>
                {
                    $"pageNumber={paginaActual}",
                    $"pageSize={tamanioPagina}"
                };

                if (fechaInicio.HasValue)
                    queryParams.Add($"fechaInicio={Uri.EscapeDataString(fechaInicio.Value.ToString("yyyy-MM-dd"))}");
                if (fechaFin.HasValue)
                    queryParams.Add($"fechaFin={Uri.EscapeDataString(fechaFin.Value.ToString("yyyy-MM-dd"))}");
                if (idCliente.HasValue)
                    queryParams.Add($"IdCliente={idCliente.Value}");

                var url = $"api/pedidos?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetFromJsonAsync<ResultadoPedidos>(url);
                return response ?? new ResultadoPedidos
                { 
                    Pedidos = new List<Pedido>(),
                    Total = 0
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener pedidos paginadas: {ex.Message}");
                return new ResultadoPedidos
                {
                    Pedidos = new List<Pedido>(),
                    Total = 0
                };
            }
        }

        public async Task<List<Pedido>> ObtenerPedidosAsync(
        int? idCliente = null,
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int paginaActual = 1,
        int tamanioPagina = 10)
        {
            try
            {
                // Asegúrate que esta URL coincida con tu API
                string url = "api/pedidos";

                var queryParams = new List<string>();
                queryParams.Add($"paginaActual={paginaActual}");
                queryParams.Add($"tamanioPagina={tamanioPagina}");

                if (idCliente.HasValue)
                    queryParams.Add($"idCliente={idCliente.Value}");

                if (fechaInicio.HasValue)
                    queryParams.Add($"fechaInicio={Uri.EscapeDataString(fechaInicio.Value.ToString("yyyy-MM-dd"))}");

                if (fechaFin.HasValue)
                    queryParams.Add($"fechaFin={Uri.EscapeDataString(fechaFin.Value.ToString("yyyy-MM-dd"))}");

                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);

                Console.WriteLine($"Requesting URL: {url}"); // Para debugging

                var response = await _httpClient.GetFromJsonAsync<List<Pedido>>(url);
                return response ?? new List<Pedido>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ObtenerPedidosAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Pedido?> ObtenerPedidoPorIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Pedido>($"api/pedidos/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        public async Task<PedidoResultado> CrearPedidoAsync(PedidoDTO pedido)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pedidos", pedido);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PedidoResultado>();
            return result ?? throw new Exception("No se pudo crear el pedido");
        }

        public async Task<PedidoDTO> ModificarPedidoAsync(Pedido pedido)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/pedidos/{pedido.Id}", pedido);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<PedidoDTO>() ??
                    throw new Exception("No se pudo modificar el pedido");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar pedido: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarPedidoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pedidos/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ActualizarEstadoPedidoAsync(int idPedido, string nuevoEstado)
        {
            try
            {
                // Normalizar el estado: primera letra mayúscula, resto minúsculas
                nuevoEstado = char.ToUpper(nuevoEstado[0]) + nuevoEstado.Substring(1).ToLower();

                var actualizacion = new EstadoPedidoDTO { Estado = nuevoEstado };
                Console.WriteLine($"Enviando actualización de estado: {nuevoEstado} para pedido {idPedido}"); // Para debugging

                var response = await _httpClient.PatchAsJsonAsync($"api/pedidos/{idPedido}/estado", actualizacion);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error del servidor: {error}"); // Para debugging
                    throw new Exception($"Error del servidor: {error}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar estado del pedido: {ex.Message}");
                throw;
            }
        }
    }
}
