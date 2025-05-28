using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using ApiECommerce.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace ECommerceWebAppFrontend.Services
{
    public class ProveedorService
    {
        private readonly HttpClient http;

        public ProveedorService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<ResultadoProveedores> ObtenerProveedoresAsync(string filtro, int pagina, int porPagina)
        {
            var response = await http.GetFromJsonAsync<ResultadoProveedores>($"api/Proveedores?nombre={filtro}&pageNumber={pagina}&pageSize={porPagina}");
            return response ?? new ResultadoProveedores();
        }

        public async Task CrearProveedorAsync(Proveedor proveedor) =>
            await http.PostAsJsonAsync("api/Proveedores", proveedor);

        public async Task ModificarProveedorAsync(Proveedor proveedor) =>
            await http.PutAsJsonAsync($"api/Proveedores/{proveedor.Id}", proveedor);

        public async Task EliminarProveedorAsync(int id) =>
            await http.DeleteAsync($"api/Proveedores/{id}");        
    }
}