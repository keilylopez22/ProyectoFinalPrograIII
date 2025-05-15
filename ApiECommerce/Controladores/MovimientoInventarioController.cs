using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;
using ApiECommerce.DTOs; // Para tus DTOs (asegúrate de que el namespace sea correcto)
using System.Collections.Generic;
using System.Threading.Tasks; 


namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoInventarioController  : ControllerBase
    {
        private readonly IMovimientosInventarioServicio _movimientoInventarioServicio;
        

        public MovimientoInventarioController(IMovimientosInventarioServicio movimientoInventarioServicio)
        {
            _movimientoInventarioServicio = movimientoInventarioServicio;
        }

        [HttpGet()]
        public async Task<IActionResult> ObtenerMovimientos(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null
        )
        {
            var resultado = await _movimientoInventarioServicio.ObtenerPedidosAsync(fechaInicio, fechaFin);
            return Ok(resultado);
        }
    }
}