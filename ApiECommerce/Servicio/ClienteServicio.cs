using ApiECommerce.Modelo; 
using ApiECommerce.Data;  
using Microsoft.EntityFrameworkCore; 
using ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs;

namespace ApiECommerce.Servicio
{
    public class ClienteServicio : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoClientes> ObtenerClientesAsync(string? nombre= null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.clientes.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(c => c.Nombre.Contains(nombre));

            var total = query.Count();
            var clientes = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var resultado = new ResultadoClientes
            {
                Clientes = clientes,
                Total = total
            };
            return resultado;
            
        }

        public async Task<Cliente> ObtenerClienteAsync(int id)
        {
            return await _context.clientes.FindAsync(id);
        }

        public async Task<bool> CrearClienteAsync(Cliente cliente)
        {
            if (cliente == null)
            {
                return false; // O podrías lanzar una excepción
            }

            _context.clientes.Add(cliente);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            if (cliente == null || !await _context.clientes.AnyAsync(c => c.Id == cliente.Id))
            {
                return false;
            }

            _context.Entry(cliente).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EliminarClienteAsync(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null)
            {
                return false;
            }

            _context.clientes.Remove(cliente);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}