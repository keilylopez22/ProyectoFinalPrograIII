using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs; // Para tus DTOs (asegúrate de que el namespace sea correcto)

namespace ApiECommerce.Controladores
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
        public async Task<ActionResult<ResultadoPaginadoProductoDTO>> GetProductos(
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
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            if (producto == null)
            {
            return NotFound();
            }
            return Ok(producto);
        }


        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] ProductoDTO dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Existencias = dto.Existencias,
                Precio = dto.Precio,
                IdCategoria = dto.IdCategoria
            };
            if (await _productoService.CrearProductosAsync(producto))
            {
                return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
            }
            return BadRequest("Error al crear el producto.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ProductoDTO dto)
        {
            var producto = new Producto
            {
                Id = id,
                Nombre = dto.Nombre,
                Existencias = dto.Existencias,
                Precio = dto.Precio,
                IdCategoria = dto.IdCategoria
            };

            // Verificar si el ID en la ruta coincide con el ID del producto
        
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