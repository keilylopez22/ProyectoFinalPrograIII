using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using ApiECommerce.Data; // Para ApplicationDbContext
using ApiECommerce.IServices; // Para tus interfaces de servicio
using ApiECommerce.DTOs; // Para tus DTOs (asegúrate de que el namespace sea correcto)
using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ApiECommerce.Controladores; // Para tus controladores (asegúrate de que el namespace sea correcto)

namespace ApiECommerce.Servicio
{
    public interface ICategoriaServicio
    {
        Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();
        Task<Categoria> ObtenerCategoriasAsync(int id);
        Task<bool> ActualizarCategoriaAsync(Categoria categoria);
        Task<bool> CrearCategoriaAsync(Categoria categoria);
        Task<bool> EliminarCategoriaAsync(int id);
    }

    public class CategoriaServicio : ICategoriaServicio
    {
        private readonly ApplicationDbContext _context;

        public CategoriaServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.categorias.ToListAsync();
        }
        public async Task<Categoria> ObtenerCategoriasAsync(int id)
        {
            return await _context.categorias.FindAsync(id);
        }
        public async Task<bool> CrearCategoriaAsync(Categoria categoria)
        {
            _context.categorias.Add(categoria);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> ActualizarCategoriaAsync(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return false;
            }
            _context.categorias.Remove(categoria);
            return await _context.SaveChangesAsync() > 0;
        }   
    }
}
