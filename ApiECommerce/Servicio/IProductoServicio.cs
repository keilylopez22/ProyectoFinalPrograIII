using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using Microsoft.EntityFrameworkCore; 
using ApiECommerce.DTOs;

namespace ApiECommerce.Servicio
{
    public interface IProductoServicio
    {
        Task<ResultadoPaginadoProductoDTO> ObtenerProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 10);
        Task<Producto> ObtenerProductoPorIdAsync(int id);
        Task<bool> CrearProductosAsync(Producto producto);
        Task<bool> ActualizarProductosAsync(Producto producto);
        Task<bool> EliminarProductosAsync(int id);
    }

    public class ProductoServicio : IProductoServicio
    {
        private readonly ApplicationDbContext _context;

        public ProductoServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoPaginadoProductoDTO> ObtenerProductosAsync(int? categoriaId = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.productos.AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(p => p.IdCategoria.HasValue && p.IdCategoria == categoriaId.Value);

            var total = await query.CountAsync();

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Existencias = p.Existencias,
                    IdCategoria = p.IdCategoria,
                    ImagenUrl = p.ImagenUrl ?? "",
                    Descripcion = p.Descripcion ?? ""
                })
                .ToListAsync();

            return new ResultadoPaginadoProductoDTO
            {
                Productos = productos,
                TotalElementos = total,
                PaginaActual = pageNumber,
                ElementosPorPagina = pageSize
            };
        }

        public async Task<Producto> ObtenerProductoPorIdAsync(int id)
        {
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