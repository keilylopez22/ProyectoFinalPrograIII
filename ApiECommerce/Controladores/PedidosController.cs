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
    public class PedidosController : ControllerBase
    {
       private readonly IPedidosServicio _pedidosServicio;

    public PedidosController(IPedidosServicio pedidosServicio)
        {
            _pedidosServicio = pedidosServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _pedidosServicio.ObtenerPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _pedidosServicio.ObtenerPedidosAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }
        
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



        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarPedidos(int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest("El ID del pedido no coincide con el ID de la ruta.");
            }

            if (await _pedidosServicio.ActualizarPedidosAsync(pedido))
            {
                return NoContent(); // Indica que la actualización fue exitosa (sin devolver contenido)
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            if (await _pedidosServicio.EliminarPedidosAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }

        //para el cambio de estado del pedido
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