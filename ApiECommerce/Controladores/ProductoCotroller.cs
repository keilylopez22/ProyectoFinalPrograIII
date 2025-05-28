using Microsoft.AspNetCore.Mvc; 
using ApiECommerce.Modelo; 
using ApiECommerce.Data;  
using ApiECommerce.Servicio; 
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs; 

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

        /// <summary>
        /// Obtiene una lista paginada de productos. Se puede filtrar por categoría.
        /// </summary>
        /// <param name="categoriaId">ID de la categoría para filtrar (opcional).</param>
        /// <param name="pageNumber">Número de página (por defecto 1).</param>
        /// <param name="pageSize">Tamaño de página (por defecto 10).</param>
        /// <returns>Una lista paginada de productos.</returns>
        [HttpGet]
        public async Task<ActionResult<ResultadoPaginadoProductoDTO>> GetProductos(
            [FromQuery] int? categoriaId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var productos = await _productoService.ObtenerProductosAsync(categoriaId, pageNumber, pageSize);
            return Ok(productos);
        }

        /// <summary>
        /// Obtiene los detalles de un producto específico por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>El producto correspondiente si se encuentra; de lo contrario, NotFound.</returns>
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

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="dto">Datos del producto a crear.</param>
        /// <returns>El producto creado y su ubicación si fue exitoso; de lo contrario, un error.</returns>
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

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="dto">Datos actualizados del producto.</param>
        /// <returns>NoContent si se actualiza correctamente, NotFound si no se encuentra, o BadRequest si hay error de validación.</returns>
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

            if (id != producto.Id)
            {
                return BadRequest("El ID del producto no coincide con el ID de la ruta.");
            }

            if (await _productoService.ActualizarProductosAsync(producto))
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>NoContent si se elimina correctamente; NotFound si no se encuentra.</returns>
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
