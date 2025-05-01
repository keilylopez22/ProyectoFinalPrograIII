using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ProyectoFinal_PrograIII.DTOs;

namespace ApiECommerce.Servicio
{
    public interface IPedidosServicio
    {
        Task<IEnumerable<Pedido>> ObtenerPedidosAsync();
        Task<Pedido> ObtenerPedidosAsync(int id);
        Task<bool> CrearPedidosAsync(Pedido pedido);
        Task<bool> ActualizarPedidosAsync(Pedido pedido);
        Task<bool> EliminarPedidosAsync(int id);

        Task<IEnumerable<Pedido>> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProducto = null,
            int? IdCliente = null,
            int? IdProveedor = null);


            //para cambiar el estado del pedido
            Task<ResultadoCambioEstado> CambiarEstadoPedidoAsync(int id, string nuevoEstado);

        
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

        //Aplicar Filtros
        public async Task<IEnumerable<Pedido>> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProducto = null,
            int? IdCliente = null,
            int? IdProveedor = null)
        {
        var query = _context.pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(dp => dp.Producto) // Asumiendo que DetallesPedido tiene una relación con Producto
                .AsQueryable();

            // Filtro por rango de fechas (periodo de tiempo)
            if (fechaInicio.HasValue)
                query = query.Where(p => p.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(p => p.Fecha <= fechaFin.Value);

            // Filtro por cliente
            if (IdCliente.HasValue)
                query = query.Where(p => p.IdCliente == IdCliente.Value);

            // Filtro por producto
            if (IdProducto.HasValue)
                query = query.Where(p => p.DetallesPedido.Any(dp => dp.IdProductos == IdProducto.Value));

            // Filtro por proveedor
            
                       
            return await query.ToListAsync();
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

        //para el cambio de estado del pedido
        public async Task<ResultadoCambioEstado> CambiarEstadoPedidoAsync(int id, string nuevoEstado)
        {
            var pedido = await _context.pedidos.FindAsync(id);
            if (pedido == null)
            {
                return new ResultadoCambioEstado
                {
                    Exitoso = false,
                    Mensaje = "Pedido no encontrado."
                };
            }

            var estadoActual = pedido.Estado?.Trim().ToLower();
            nuevoEstado = nuevoEstado?.Trim().ToLower();

            var estadosPermitidos = new[] { "pendiente", "enviado", "entregado", "cancelado" };
            if (!estadosPermitidos.Contains(nuevoEstado))
            {
                return new ResultadoCambioEstado
                {
                    Exitoso = false,
                    Mensaje = $"Estado '{nuevoEstado}' no es válido. Los estados válidos son: Pendiente, Enviado, Entregado, Cancelado."
                };
            }

            var transicionesValidas = new Dictionary<string, List<string>>
            {
                { "pendiente", new List<string> { "enviado", "cancelado" } },
                { "enviado",   new List<string> { "entregado", "cancelado" } },
                { "entregado", new List<string>() },
                { "cancelado", new List<string>() }
            };

            if (!transicionesValidas.ContainsKey(estadoActual) || !transicionesValidas[estadoActual].Contains(nuevoEstado))
            {
                return new ResultadoCambioEstado
                {
                    Exitoso = false,
                    Mensaje = $"No se puede cambiar el estado de '{estadoActual}' a '{nuevoEstado}'."
                };
            }

            // Aplica el cambio
            pedido.Estado = char.ToUpper(nuevoEstado[0]) + nuevoEstado.Substring(1); // "enviado" → "Enviado"
            _context.pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return new ResultadoCambioEstado
            {
                Exitoso = true,
                Mensaje = "Estado del pedido actualizado exitosamente."
            };
        }
    }
}