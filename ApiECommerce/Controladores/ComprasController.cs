using Microsoft.AspNetCore.Mvc;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.Servicio;
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs;
using System;

namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly IComprasServicio _comprasServicio;

        public ComprasController(IComprasServicio comprasServicio)
        {
            _comprasServicio = comprasServicio;
        }

        /// <summary>
        /// Obtiene una lista paginada de compras filtradas por fecha o proveedor.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio opcional para el filtro.</param>
        /// <param name="fechaFin">Fecha de fin opcional para el filtro.</param>
        /// <param name="IdProveedor">ID del proveedor opcional.</param>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10).</param>
        /// <returns>Lista paginada de compras.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ResultadoCompras), 200)]
        public async Task<ActionResult<ResultadoCompras>> GetCompras(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] int? IdProveedor = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var compras = await _comprasServicio.ObtenerComprasAsync(fechaInicio, fechaFin, IdProveedor, pageNumber, pageSize);
            return Ok(compras);
        }

        /// <summary>
        /// Obtiene los detalles de una compra por su ID.
        /// </summary>
        /// <param name="id">ID de la compra.</param>
        /// <returns>Compra encontrada o error 404 si no existe.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Compra), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _comprasServicio.ObtenerComprasAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return Ok(compra);
        }

        /// <summary>
        /// Crea una nueva compra.
        /// </summary>
        /// <param name="compraDto">Datos de la compra a crear.</param>
        /// <returns>Resultado de la creación de la compra.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CompraResultado), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CompraResultado>> CrearCompras([FromBody] CompraDTO compraDto)
        {
            var resultado = await _comprasServicio.CrearComprasAsync(compraDto);
            return Ok(resultado);
        }

        /// <summary>
        /// Actualiza una compra existente.
        /// </summary>
        /// <param name="id">ID de la compra a actualizar.</param>
        /// <param name="compra">Objeto compra con los nuevos datos.</param>
        /// <returns>NoContent si se actualiza correctamente, 404 si no se encuentra.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ActualizarCompras(int id, [FromBody] Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest("El ID de la compra no coincide con el ID de la ruta.");
            }

            if (await _comprasServicio.ActualizarComprasAsync(compra))
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Elimina una compra por su ID.
        /// </summary>
        /// <param name="id">ID de la compra a eliminar.</param>
        /// <returns>NoContent si se elimina correctamente, 404 si no se encuentra.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EliminarCompra(int id)
        {
            if (await _comprasServicio.EliminarComprasAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
