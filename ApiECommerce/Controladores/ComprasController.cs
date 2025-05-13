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
    public class ComprasController : ControllerBase
    {
       private readonly IComprasServicio _comprasServicio;

    public ComprasController(IComprasServicio comprasServicio)
        {
            _comprasServicio = comprasServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras(
            [FromQuery] DateTime? fechaInicio = null,
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] int? IdProveedor = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var compras = await _comprasServicio.ObtenerComprasAsync(fechaInicio,fechaFin,IdProveedor, pageNumber, pageSize);
            return Ok(compras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _comprasServicio.ObtenerComprasAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return Ok(compra);
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> CrearCompras([FromBody] Compra compra)
        {
            if (await _comprasServicio.CrearComprasAsync(compra))
            {
                return CreatedAtAction(nameof(GetCompras), new { id = compra.Id }, compra);
            }
            return BadRequest("Error al crear la compra.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCompras(int id, [FromBody] Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest("El ID de la compra no coincide con el ID de la ruta.");
            }

            if (await _comprasServicio.ActualizarComprasAsync(compra))
            {
                return NoContent(); // Indica que la actualización fue exitosa (sin devolver contenido)
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
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