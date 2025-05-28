using Microsoft.AspNetCore.Mvc;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.Servicio;
using ApiECommerce.IServices;
using ApiECommerce.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiECommerce.Controladores
{
    /// <summary>
    /// Controlador para la gestión de movimientos de inventario.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientoInventarioController : ControllerBase
    {
        private readonly IMovimientosInventarioServicio _movimientoInventarioServicio;

        /// <summary>
        /// Constructor del controlador de movimientos de inventario.
        /// </summary>
        /// <param name="movimientoInventarioServicio">Servicio para la gestión de movimientos de inventario.</param>
        public MovimientoInventarioController(IMovimientosInventarioServicio movimientoInventarioServicio)
        {
            _movimientoInventarioServicio = movimientoInventarioServicio;
        }

        /// <summary>
        /// Obtiene los movimientos de inventario filtrados por fecha de inicio y fin.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio (opcional).</param>
        /// <param name="fechaFin">Fecha de fin (opcional).</param>
        /// <returns>Una lista de movimientos de inventario.</returns>
        /// <response code="200">Retorna la lista de movimientos de inventario.</response>
        /// <response code="500">Si ocurre un error interno.</response>
        [HttpGet]
        [ProducesResponseType(typeof(MovimientoInventarioResultado), 200)]
        [ProducesResponseType(500)]
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
