using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.Servicio;
using ApiECommerce.IServices;

namespace ApiECommerce.Controllers
{
    /// <summary>
    /// Controlador para administrar las categorías de productos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaServicio _categoriaServicio;

        public CategoriasController(ICategoriaServicio categoriaServicio)
        {
            _categoriaServicio = categoriaServicio;
        }

        /// <summary>
        /// Obtiene todas las categorías de productos.
        /// </summary>
        /// <returns>Una lista de categorías.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            var categorias = await _categoriaServicio.ObtenerCategoriasAsync();
            return Ok(categorias);
        }

        /// <summary>
        /// Obtiene una categoría específica por su ID.
        /// </summary>
        /// <param name="id">ID de la categoría.</param>
        /// <returns>La categoría solicitada.</returns>
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

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="categoria">Datos de la categoría a crear.</param>
        /// <returns>La categoría creada.</returns>
        [HttpPost]
        public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest();
            }

            var resultado = await _categoriaServicio.CrearCategoriaAsync(categoria);
            if (!resultado)
            {
                return StatusCode(500, "Error al crear la categoría");
            }
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        /// <summary>
        /// Actualiza una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a actualizar.</param>
        /// <param name="categoria">Datos actualizados de la categoría.</param>
        /// <returns>Categoría actualizada o mensaje de error.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

            var categoriaExistente = await _categoriaServicio.ObtenerCategoriasAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }

            categoriaExistente.Nombre = categoria.Nombre;

            var resultado = await _categoriaServicio.ActualizarCategoriaAsync(categoriaExistente);
            if (!resultado)
            {
                return StatusCode(500, "Error al actualizar la categoría");
            }

            return Ok(categoriaExistente);
        }

        /// <summary>
        /// Elimina una categoría por su ID.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar.</param>
        /// <returns>Código de estado que indica el resultado de la operación.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoriaExistente = await _categoriaServicio.ObtenerCategoriasAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound();
            }

            var resultado = await _categoriaServicio.EliminarCategoriaAsync(id);
            if (!resultado)
            {
                return StatusCode(500, "Error al eliminar la categoría");
            }

            return NoContent();
        }
    }
}
