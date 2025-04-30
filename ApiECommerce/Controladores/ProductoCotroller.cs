using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ProyectoFinal_PrograIII.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ProyectoFinal_PrograIII.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ProyectoFinal_PrograIII.Servicio; // Si estás usando una capa de servicios
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
       private readonly IProductoServicio _productoService;
    

        public ProductoController(IProductoServicio productoServicio)
        {
            _productoService = productoServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
            [FromQuery] int? categoriaId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var productos = await _productoService.ObtenerProductosAsync(categoriaId, pageNumber, pageSize);
            return Ok(productos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.ObtenerProductosAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] Producto producto)
        {
            if (await _productoService.CrearProductosAsync(producto))
            {
                return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
            }
            return BadRequest("Error al crear el producto.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest("El ID del producto no coincide con el ID de la ruta.");
            }

            if (await _productoService.ActualizarProductosAsync(producto))
            {
                return NoContent(); // Indica que la actualización fue exitosa (sin devolver contenido)
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            if (await _productoService.EliminarProductosAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}