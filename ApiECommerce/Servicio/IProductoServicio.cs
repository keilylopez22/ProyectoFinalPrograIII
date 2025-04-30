using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.Data;
using Microsoft.EntityFrameworkCore; 

namespace ProyectoFinal_PrograIII.Servicio
{
    public interface IProductoServicio
    {
        Task<IEnumerable<Producto>> ObtenerProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 10);
        Task<Producto> ObtenerProductosAsync(int id);
        Task<bool> CrearProductosAsync(Producto producto);
        Task<bool> ActualizarProductosAsync(Producto producto);
        Task<bool> EliminarProductosAsync(int id);
        
 
    }
    public class ProductoServicio:IProductoServicio
    {
        private readonly ApplicationDbContext _context;

        public ProductoServicio(ApplicationDbContext context)
        {
            _context = context;

        }
        
        public async Task<IEnumerable<Producto>> ObtenerProductosAsync(int? IdCategoria = null, int pageNumber = 1, int pageSize = 10)
        {
            // Construir la consulta base incluyendo la categoría
            var query = _context.productos
                .Include(p => p.Categoria)
                .AsQueryable();

            // Filtro por categoría si se proporciona un ID
            if (IdCategoria.HasValue)
            {
                query = query.Where(p => p.IdCategoria == IdCategoria.Value);
            }

            // Aplicar paginación
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Ejecutar la consulta
            return await query.ToListAsync();
        }

        public async Task<Producto> ObtenerProductosAsync(int id)
        {
            // Incluir la categoría relacionada al producto
            // y buscar el producto por su ID   

            return await _context.productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            

        }
        public async Task<bool> CrearProductosAsync(Producto producto)
        {
            await _context.productos.AddAsync(producto);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ActualizarProductosAsync(Producto producto)
        { 
             if (producto == null || !await _context.productos.AnyAsync(c => c.Id == producto.Id))
            {
                return false;
            }

            _context.Entry(producto).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }
        public async Task<bool> EliminarProductosAsync(int id)
        {
            var producto = await _context.productos.FindAsync(id);
            if (producto != null)
            {
                _context.productos.Remove(producto);
                await _context.SaveChangesAsync();
                return true;

            }
            return false;

        }

        
    }
}