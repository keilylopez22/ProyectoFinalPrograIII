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
    public interface IProveedoresServicio
    {
        Task<ResultadoProveedores> ObtenerProveedoresAsync(string? nombre = null, int pageNumber = 1, int pageSize = 10);
        Task<Proveedor> ObtenerProveedorPorIdAsync(int id);
        Task<bool> CrearProveedorAsync(Proveedor Proveedor);
        Task<bool> ActualizarProveedorAsync(Proveedor proveedor);
        Task<bool> EliminarProveedorAsync(int id);
    }

    public class ProveedorServicio : IProveedoresServicio
    {
        private readonly ApplicationDbContext _context;

        public ProveedorServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoProveedores> ObtenerProveedoresAsync(string? nombre = null, int pageNumber = 1, int pageSize = 10)
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

        public async Task<Proveedor> ObtenerProveedorPorIdAsync(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            
            return proveedor;
        }

        public async Task<bool> CrearProveedorAsync(Proveedor proveedor)
        {
             _context.proveedores.Add(proveedor);
            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetProveedor", new { id = proveedor.Id }, proveedor);
            return true;
        }

        public async Task<bool> ActualizarProveedorAsync(Proveedor proveedor)
        {
            if (proveedor == null || !await _context.proveedores.AnyAsync(c => c.Id == proveedor.Id))
            {
                return false; // Proveedor no encontrado
            }
            _context.Entry(proveedor).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync();
            return result > 0; // Retorna true si se actualizó correctamente
        }

        public async Task<bool> EliminarProveedorAsync(int id)
        {
            var proveedor = await _context.proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}