using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using ApiECommerce.Modelo;
using ApiECommerce.DTOs;

namespace ECommerceWebAppFrontend.Services
{
    public interface ICompraService
    {
        Task<ResultadoCompras> ObtenerComprasPaginadasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? idProveedor = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<Compra?> ObtenerCompraAsync(int id);
        Task<CompraResultado?> CrearCompraAsync(CompraDTO compra);
        Task<bool> ActualizarCompraAsync(Compra compra);
        Task<bool> EliminarCompraAsync(int id);
    }

    public class CompraService : ICompraService
    {
        private readonly HttpClient _httpClient;

        public CompraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultadoCompras> ObtenerComprasPaginadasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? idProveedor = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var queryParams = new List<string>
                {
                    $"pageNumber={pageNumber}",
                    $"pageSize={pageSize}"
                };

                if (fechaInicio.HasValue)
                    queryParams.Add($"fechaInicio={Uri.EscapeDataString(fechaInicio.Value.ToString("yyyy-MM-dd"))}");
                if (fechaFin.HasValue)
                    queryParams.Add($"fechaFin={Uri.EscapeDataString(fechaFin.Value.ToString("yyyy-MM-dd"))}");
                if (idProveedor.HasValue)
                    queryParams.Add($"IdProveedor={idProveedor.Value}");

                var url = $"api/compras?{string.Join("&", queryParams)}";
                var response = await _httpClient.GetFromJsonAsync<ResultadoCompras>(url);
                return response ?? new ResultadoCompras 
                { 
                    Compras = new List<Compra>(),
                    Total = 0
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener compras paginadas: {ex.Message}");
                return new ResultadoCompras
                {
                    Compras = new List<Compra>(),
                    Total = 0
                };
            }
        }

        public async Task<Compra?> ObtenerCompraAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Compra>($"api/compras/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener compra: {ex.Message}");
                return null;
            }
        }

        public async Task<CompraResultado?> CrearCompraAsync(CompraDTO compra)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/compras", compra);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CompraResultado>();
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear compra: {errorContent}");
                return new CompraResultado
                {
                    Exito = false,
                    Mensaje = errorContent
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear compra: {ex.Message}");
                return new CompraResultado
                {
                    Exito = false,
                    Mensaje = ex.Message
                };
            }
        }

        public async Task<bool> ActualizarCompraAsync(Compra compra)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/compras/{compra.Id}", compra);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar compra: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EliminarCompraAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/compras/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar compra: {ex.Message}");
                return false;
            }
        }
    }
}
