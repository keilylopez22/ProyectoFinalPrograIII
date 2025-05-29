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
    /// Controlador para gestionar los pedidos (órdenes de compra) realizados por los clientes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidosServicio _pedidosServicio;
        private readonly IKafkaProductorServicio _kafkaProductorServicio;

        /// <summary>
        /// Constructor del controlador de pedidos.
        /// </summary>
        public PedidosController(IPedidosServicio pedidosServicio, IKafkaProductorServicio kafkaProductorServicio)
        {
            _pedidosServicio = pedidosServicio;
            _kafkaProductorServicio = kafkaProductorServicio;
        }

        /// <summary>
        /// Obtiene una lista de pedidos(ventas) filtrados por fecha, cliente y con paginación.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del filtro (opcional).</param>
        /// <param name="fechaFin">Fecha de fin del filtro (opcional).</param>
        /// <param name="IdCliente">ID del cliente (opcional).</param>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10).</param>
        /// <returns>Una lista paginada de pedidos(ventas).</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] int? IdCliente = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var pedidos = await _pedidosServicio.ObtenerPedidosAsync(
                fechaInicio,
                fechaFin,
                IdCliente,
                pageNumber,
                pageSize
            );
            return Ok(pedidos);
        }

        /// <summary>
        /// Obtiene los detalles de un pedido(ventas) por su ID.
        /// </summary>
        /// <param name="id">ID del pedido.</param>
        /// <returns>El pedido solicitado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _pedidosServicio.ObtenerPedidosAsync(id);
            if (pedido == null)
            {
                return NotFound($"No se encontró el pedido con ID {id}");
            }
            return Ok(pedido);
        }

        /// <summary>
        /// Crea un nuevo pedido(ventas) y lo envía para procesamiento mediante Kafka (versión asincrónica).
        /// </summary>
        /// <param name="pedido">DTO con los datos del pedido.</param>
        /// <returns>Resultado de aceptación del pedido.</returns>
        [HttpPost("V2")]
        public async Task<IActionResult> CrearPedidoKafka([FromBody] PedidoKafkaDTO pedido)
        {
            await _kafkaProductorServicio.EnviarPedidoAsync(pedido);
            return Accepted("El pedido fue enviado para su procesamiento.");
        }

        /// <summary>
        /// Crea un nuevo pedido(ventas) en el sistema.
        /// </summary>
        /// <param name="pedidoDto">DTO con los datos del pedido(ventas).</param>
        /// <returns>El pedido creado.</returns>
        [HttpPost]
        public async Task<ActionResult> CrearPedidos([FromBody] PedidoDTO pedidoDto)
        {
            var resultado = await _pedidosServicio.CrearPedidosAsync(pedidoDto);

            if (resultado.Exito && resultado.Pedido != null)
            {
                return CreatedAtAction(nameof(GetPedido), new { id = resultado.Pedido.Id }, resultado.Pedido);
            }

            return BadRequest(resultado.Mensaje);
        }

        /// <summary>
        /// Actualiza un pedido(venta) existente por su ID.
        /// </summary>
        /// <param name="id">ID del pedido a actualizar.</param>
        /// <param name="pedido">Datos del pedido actualizados.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedidos(int id, [FromBody] PedidoUpdateDTO pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest("El ID del pedido no coincide con el ID de la ruta.");
            }

            if (await _pedidosServicio.ActualizarPedidosAsync(pedido))
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Elimina un pedido(venta) del sistema.
        /// </summary>
        /// <param name="id">ID del pedido a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            if (await _pedidosServicio.EliminarPedidosAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Cambia el estado de un pedido(venta).
        /// </summary>
        /// <param name="id">ID del pedido.</param>
        /// <param name="estado">Nuevo estado del pedido.</param>
        /// <returns>Mensaje del resultado.</returns>
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromBody] EstadoPedidoDTO estado)
        {
            var resultado = await _pedidosServicio.CambiarEstadoPedidoAsync(id, estado.Estado);

            if (resultado.Exitoso)
                return Ok(new { mensaje = resultado.Mensaje });

            return BadRequest(new { error = resultado.Mensaje });
        }
    }
}
