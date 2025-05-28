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
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProveedoresServicio _proveedoresServicio;

        public ProveedoresController(ApplicationDbContext context, IProveedoresServicio proveedoresServicio)
        {
            _proveedoresServicio = proveedoresServicio;  
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista paginada de proveedores. Puede filtrarse por nombre.
        /// </summary>
        /// <param name="nombre">Nombre del proveedor (opcional)</param>
        /// <param name="pageNumber">Número de página</param>
        /// <param name="pageSize">Tamaño de página</param>
        /// <returns>Lista paginada de proveedores</returns>
        [HttpGet]
        public async Task<ActionResult<ResultadoProveedores>> GetProveedores(
            [FromQuery] string? nombre,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var resultado = await _proveedoresServicio.ObtenerProveedoresAsync(nombre, pageNumber, pageSize);
            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado);
        }

        /// <summary>
        /// Obtiene un proveedor por su ID.
        /// </summary>
        /// <param name="id">ID del proveedor</param>
        /// <returns>Proveedor encontrado</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _proveedoresServicio.ObtenerProveedorPorIdAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor);
        }

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        /// <param name="proveedor">Datos del proveedor</param>
        /// <returns>Proveedor creado</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            var resultado = await _proveedoresServicio.CrearProveedorAsync(proveedor);
            if (!resultado)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
        }

        /// <summary>
        /// Actualiza un proveedor existente.
        /// </summary>
        /// <param name="id">ID del proveedor a actualizar</param>
        /// <param name="proveedor">Datos actualizados del proveedor</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest();
            }

            var resultado = await _proveedoresServicio.ActualizarProveedorAsync(proveedor);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Elimina un proveedor por su ID.
        /// </summary>
        /// <param name="id">ID del proveedor a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var resultado = await _proveedoresServicio.EliminarProveedorAsync(id);
            if (!resultado)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// Verifica si un proveedor existe por su ID.
        /// </summary>
        /// <param name="id">ID del proveedor</param>
        /// <returns>True si existe, False si no</returns>
        private bool ProveedorExists(int id)
        {
            return _context.proveedores.Any(e => e.Id == id);
        }
    }
}
