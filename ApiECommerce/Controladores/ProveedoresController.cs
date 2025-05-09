using Microsoft.AspNetCore.Mvc; 
using ApiECommerce.Modelo; 
using ApiECommerce.Data;  
using ApiECommerce.Servicio; 
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
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
        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
            
        }
           // Obtener todos los proveedores (GET)
        

        [HttpGet]// Ruta: /api/Proveedores
        [Produces("application/json")]  // Esto le indica a Swagger que la respuesta será JSON
        public async Task<ActionResult<ResultadoProveedores>>GetProveedores(
            [FromQuery] string? nombre,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = _context.proveedores.AsQueryable();

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
            return resultado;
        }
      
        [HttpGet("{id}")] // Ruta: /api/Proveedores/{id} - Así Swagger sabrá diferenciarlos
        [Produces("application/json")]  // Esto le indica a Swagger que la respuesta será JSON
        
        //obtener un proveedor por Id Get
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return proveedor;

        }

        //crear un proveedor (POST)
        [HttpPost]
        [Produces("application/json")]  // Esto le indica a Swagger que la respuesta será JSON
        [Consumes("application/json")] // Indica que espera recibir un JSON
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            _context.proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetProveedor", new { id = proveedor.Id }, proveedor);
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
            _context.Entry(proveedor).State = EntityState.Modified;
            
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
            return NoContent();

        }
        //eliminar un proveedor (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            _context.proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool ProveedorExists(int id)
        {
            return _context.proveedores.Any(e => e.Id == id);
        }



    }

}