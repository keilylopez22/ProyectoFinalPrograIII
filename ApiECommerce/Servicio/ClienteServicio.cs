using ProyectoFinal_PrograIII.Modelo; // Si tus modelos están en este namespace
using ProyectoFinal_PrograIII.Data;  // Si tu ApplicationDbContext está en este namespace
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.Servicio
{
    public class ClienteServicio : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesAsync()
        {
            return await _context.clientes.ToListAsync();
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