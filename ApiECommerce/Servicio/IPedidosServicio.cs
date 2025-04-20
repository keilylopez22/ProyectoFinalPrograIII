using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.Data;

namespace ProyectoFinal_PrograIII.Servicio
{
    public interface IPedidosServicio
    {
        Task<IEnumerable<Pedido>> ObtenerPedidosAsync();
        Task<Pedido> ObtenerPedidosAsync(int id);
        Task<bool> CrearPedidosAsync(Pedido pedido);
        Task<bool> ActualizarPedidosAsync(Pedido pedido);
        Task<bool> EliminarPedidosAsync(int id);
    }


    public class PedidoServicio:IPedidosServicio
    {
         private readonly ApplicationDbContext _context;
         public PedidoServicio(ApplicationDbContext context)
         {
            _context = context;

         }
        public async Task<IEnumerable<Pedido>> ObtenerPedidosAsync()
        {

            return await _context.pedidos.
            Include(p => p.Cliente).
            Include(p  => p.DetallesPedido).
            ToListAsync();
        }
        public async Task<Pedido> ObtenerPedidosAsync(int id)
        {
            
            return await _context.pedidos.
            Include(p => p.Cliente).
            Include(p  => p.DetallesPedido).
            FirstOrDefaultAsync(p => p.Id == id );
            
        }
        public async Task<bool> CrearPedidosAsync(Pedido pedido)
        {
            if(pedido == null)
            {
                return false;
            }

            await _context.pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ActualizarPedidosAsync(Pedido pedido)
        {
            _context.pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> EliminarPedidosAsync(int id)
        {
            var pedido = await _context.pedidos.FindAsync(id);

            if ( pedido != null)
            {
                _context.pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

 
    }
}