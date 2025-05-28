using Microsoft.AspNetCore.Mvc;  
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.Servicio;

namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]  // Esto hará que las URLs sean /api/reportes
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteServicio _reporteServicio;

        public ReportesController(IReporteServicio reporteServicio)
        {
            // Inyectar instancia del servicio para que se pueda utilizar dentro del controlador  
            _reporteServicio = reporteServicio;
        }

        /// <summary>
        /// Genera un reporte de compras en formato Excel.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para filtrar las compras (opcional).</param>
        /// <param name="fechaFin">Fecha de fin para filtrar las compras (opcional).</param>
        /// <param name="Idproducto">ID del producto a filtrar (opcional).</param>
        /// <param name="IdProveedor">ID del proveedor a filtrar (opcional).</param>
        /// <returns>Archivo Excel con el reporte de compras.</returns>
        /// <response code="200">Archivo generado correctamente.</response>
        [HttpGet("compras")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> GetReporteCompras(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? Idproducto,
            [FromQuery] int? IdProveedor)
        {
            var archivo = await _reporteServicio.GenerarReporteCompras(fechaInicio, fechaFin, Idproducto, IdProveedor);
            return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCompras.xlsx");
        }

        /// <summary>
        /// Genera un reporte de pedidos en formato Excel.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio para filtrar los pedidos (opcional).</param>
        /// <param name="fechaFin">Fecha de fin para filtrar los pedidos (opcional).</param>
        /// <param name="Idproducto">ID del producto a filtrar (opcional).</param>
        /// <param name="Idcliente">ID del cliente a filtrar (opcional).</param>
        /// <returns>Archivo Excel con el reporte de pedidos.</returns>
        /// <response code="200">Archivo generado correctamente.</response>
        [HttpGet("pedidos")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        public async Task<IActionResult> GetReportePedidos(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? Idproducto,
            [FromQuery] int? Idcliente)
        {
            var archivo = await _reporteServicio.GenerarReportePedidos(fechaInicio, fechaFin, Idproducto, Idcliente, null);
            return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePedidos.xlsx");
        }
    }
}
