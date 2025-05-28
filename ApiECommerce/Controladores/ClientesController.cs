using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Para IQueryable y LINQ
using ApiECommerce.DTOs; // Para ResultadoClientes (asegúrate de que el namespace sea correcto)

namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtiene una lista paginada de clientes, con opción de búsqueda por nombre.
        /// </summary>
        /// <param name="nombre">Nombre del cliente para filtrar (opcional).</param>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10).</param>
        /// <returns>Una lista paginada de clientes.</returns>
        [HttpGet]
        public async Task<ActionResult<ResultadoClientes>> GetClientes(
            [FromQuery] string? nombre,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var clientes = await _clienteService.ObtenerClientesAsync(nombre, pageNumber, pageSize);
            return Ok(clientes);
        }

        /// <summary>
        /// Obtiene un cliente por su identificador único.
        /// </summary>
        /// <param name="id">ID del cliente a obtener.</param>
        /// <returns>El cliente solicitado si existe.</returns>
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

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="cliente">Objeto cliente que se desea crear.</param>
        /// <returns>El cliente creado y su ubicación.</returns>
        [HttpPost]
        public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
        {
            if (await _clienteService.CrearClienteAsync(cliente))
            {
                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            return BadRequest("Error al crear cliente.");
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        /// <param name="id">ID del cliente a actualizar.</param>
        /// <param name="cliente">Objeto cliente con los nuevos datos.</param>
        /// <returns>NoContent si se actualiza correctamente; NotFound si el cliente no existe.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("El ID del cliente no coincide con el ID de la ruta.");
            }

            if (await _clienteService.ActualizarClienteAsync(cliente))
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente a eliminar.</param>
        /// <returns>NoContent si se elimina correctamente; NotFound si el cliente no existe.</returns>
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
