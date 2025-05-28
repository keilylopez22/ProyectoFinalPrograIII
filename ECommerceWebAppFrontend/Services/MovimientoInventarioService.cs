using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using ApiECommerce.DTOs;
using ApiECommerce.Modelo;

namespace ECommerceWebAppFrontend.Services
{
    public class MovimientoInventarioService
    {
        private readonly HttpClient http;

        public MovimientoInventarioService(HttpClient http)
        {
            this.http = http;
        }

       public async Task<MovimientoInventarioResultado> ObtenerMovimientosAsync(int pagina, int tamanoPagina, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var url = $"api/MovimientoInventario?pagina={pagina}&tamanoPagina={tamanoPagina}";

            if (fechaInicio.HasValue)
                url += $"&fechaInicio={fechaInicio.Value:yyyy-MM-dd}";

            if (fechaFin.HasValue)
                url += $"&fechaFin={fechaFin.Value:yyyy-MM-dd}";

            return await http.GetFromJsonAsync<MovimientoInventarioResultado>(url);
        }
    }
}
