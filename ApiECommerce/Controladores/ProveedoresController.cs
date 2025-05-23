using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs; 


namespace ApiECommerce.Controladores
//namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]  // Esto hará que las URLs sean /api/proveedores
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        //private readonly ECommersContext _context;
        private readonly ApplicationDbContext _context;
        private readonly IProveedoresServicio _proveedoresServicio;
        public ProveedoresController(ApplicationDbContext context, IProveedoresServicio proveedoresServicio)
        {
            _proveedoresServicio = proveedoresServicio;  
            
            _context = context;

        }
        // Obtener todos los proveedores (GET)


        [HttpGet]// Ruta: /api/Proveedores        
        public async Task<ActionResult<ResultadoProveedores>> GetProveedores(
            [FromQuery] string? nombre,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            /* var query = _context.proveedores.AsQueryable();

             if (!string.IsNullOrEmpty(nombre))
                 query = query.Where(c => c.Nombre.Contains(nombre));

             var total = query.Count();
             var proveedores = await query
                 .Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize)
                 .ToListAsync();

             var resultado = new ResultadoProveedores
             {
                 Proveedores = proveedores,
                 Total = total
             };
             return resultado;*/
            // Llamar al servicio para obtener los proveedores
            var resultado = await _proveedoresServicio.ObtenerProveedoresAsync(nombre, pageNumber, pageSize);
            if (resultado == null)
            {
                return NotFound();
            }
            return Ok(resultado); // Devolvemos el resultado encontrado
        }

        [HttpGet("{id}")] // Ruta: /api/Proveedores/{id} - Así Swagger sabrá diferenciarlos
        [Produces("application/json")]  // Esto le indica a Swagger que la respuesta será JSON

        //obtener un proveedor por Id Get
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            /*var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return proveedor;*/
            // Llamar al servicio para obtener el proveedor por ID
            var proveedor = await _proveedoresServicio.ObtenerProveedorPorIdAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return Ok(proveedor); // Devolvemos el proveedor encontrado|

        }

        //crear un proveedor (POST)
        [HttpPost]
        [Produces("application/json")]  // Esto le indica a Swagger que la respuesta será JSON
        [Consumes("application/json")] // Indica que espera recibir un JSON
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            /*_context.proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetProveedor", new { id = proveedor.Id }, proveedor);
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);  // Devolvemos un código 201 (Created)*/
            // Llamar al servicio para crear el proveedor
            var resultado = await _proveedoresServicio.CrearProveedorAsync(proveedor);
            if (!resultado)
            {
                return BadRequest(); // Devolvemos un código 400 (Bad Request) si no se pudo crear
            }
            return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);  // Devolvemos un código 201 (Created)
        }

        //actualizar un proveedor (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest();
            }
            /*_context.Entry(proveedor).State = EntityState.Modified;
            
            try
            { 
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();*/
            // Llamar al servicio para actualizar el proveedor
            var resultado = await _proveedoresServicio.ActualizarProveedorAsync(proveedor);
            if (!resultado)
            {
                return NotFound(); // Devolvemos un código 404 (Not Found) si no se encontró el proveedor
            }
            return NoContent(); // Devolvemos un código 204 (No Content) si se actualizó correctamente

        }
        //eliminar un proveedor (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            /*var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            _context.proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return NoContent();*/
            // Llamar al servicio para eliminar el proveedor
            var resultado = await _proveedoresServicio.EliminarProveedorAsync(id);
            if (!resultado)
            {
                return NotFound(); // Devolvemos un código 404 (Not Found) si no se encontró el proveedor
            }
            return NoContent(); // Devolvemos un código 204 (No Content) si se eliminó correctamente
        }
        private bool ProveedorExists(int id)
        {
            return _context.proveedores.Any(e => e.Id == id);
        }



    }

}