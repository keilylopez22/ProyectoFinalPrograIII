using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
       private readonly ApiECommerce.IServices.IClienteService _clienteService;

    public ClientesController(ApiECommerce.IServices.IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var clientes = await _clienteService.ObtenerClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _clienteService.ObtenerClienteAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
        {
            if (await _clienteService.CrearClienteAsync(cliente))
            {
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            return BadRequest("Error al crear cliente.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("El ID del cliente no coincide con el ID de la ruta.");
            }

            if (await _clienteService.ActualizarClienteAsync(cliente))
            {
                return NoContent(); // Indica que la actualización fue exitosa (sin devolver contenido)
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            if (await _clienteService.EliminarClienteAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}