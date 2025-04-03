using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ProyectoFinal_PrograIII.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ProyectoFinal_PrograIII.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ProyectoFinal_PrograIII.Servicio; // Si estás usando una capa de servicios
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Necesario para DbContext, EntityState y DbUpdateConcurrencyException

namespace ProyectoFinal_PrograIII.Controladores
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
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            return await _context.proveedores.ToListAsync();
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