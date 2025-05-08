using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiECommerce.Modelo;// Para tus modelos (asegúrate de que el namespace sea correcto)
using ApiECommerce.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ApiECommerce.Servicio; // Si estás usando una capa de servicios
using ApiECommerce.IServices;


namespace ApiECommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaServicio _categoriaServicio;

        public CategoriasController(ICategoriaServicio categoriaServicio)
        {
            _categoriaServicio = categoriaServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            var categorias = await _categoriaServicio.ObtenerCategoriasAsync();
            return Ok(categorias);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _categoriaServicio.ObtenerCategoriasAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }
        [HttpPost]
        public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest();
            }

            // Aquí puedes agregar lógica para guardar la categoría en la base de datos
            // Por ejemplo, usando el servicio de categoría
            var resultado = await _categoriaServicio.CrearCategoriaAsync(categoria);
            if (!resultado)
            {
                return StatusCode(500, "Error al crear la categoría");
            }
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            // Aquí puedes agregar lógica para actualizar la categoría en la base de datos
            // Por ejemplo, usando el servicio de categoría
            var categoriaExistente = await _categoriaServicio.ObtenerCategoriasAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }
            // Actualiza la categoría existente con los nuevos valores
            categoriaExistente.Nombre = categoria.Nombre;
            
            // Guarda los cambios en la base de datos
            var resultado = await _categoriaServicio.ActualizarCategoriaAsync(categoriaExistente);
            if (!resultado)
            {
                return StatusCode(500, "Error al actualizar la categoría");
            }
            // Si la actualización fue exitosa, devuelve un código 204 No Content
            // o puedes devolver la categoría actualizada si lo prefieres 
            return Ok(categoriaExistente);              

            
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            // Aquí puedes agregar lógica para eliminar la categoría de la base de datos
            // Por ejemplo, usando el servicio de categoría
            var categoriaExistente = await _categoriaServicio.ObtenerCategoriasAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }

            // Elimina la categoría existente
            var resultado = await _categoriaServicio.EliminarCategoriaAsync(id);

            if (!resultado)
            {
                return StatusCode(500, "Error al eliminar la categoría");
            }
            // Si la eliminación fue exitosa, devuelve un código 204 No Content
            // o puedes devolver un mensaje de éxito si lo prefieres
            

            return NoContent();
        }
        
    }
}

