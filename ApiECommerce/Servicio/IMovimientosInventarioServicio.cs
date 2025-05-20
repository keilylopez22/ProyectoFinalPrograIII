using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiECommerce.Servicio
{
    public interface IMovimientosInventarioServicio
    {
        Task<MovimientoInventarioResultado> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null
        );
        Task<bool> RegistrarMovimientoCompraAsync(int idProducto, int cantidad, int idCompra, string? nota = null); 
        Task<bool> RegistrarMovimientoPedidoAsync(int idProducto, int cantidad, int idPedido, string? nota = null);
             
        
    }
    public class MovimientosInventarioServicio:IMovimientosInventarioServicio
    {

        private readonly ApplicationDbContext _context;
        public MovimientosInventarioServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MovimientoInventarioResultado> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null
        )
        {
            var query = _context.movimientosInventario
                .Include(m => m.Producto) // Incluye la entidad relacionada Producto
                .AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(m => m.FechaMovimiento >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(m => m.FechaMovimiento <= fechaFin.Value);
            }

            var totalRegistros = await query.CountAsync();

            var movimientos = await query.ToListAsync();

            return new MovimientoInventarioResultado
            {
                Pagina = 1,
                TamanoPagina = totalRegistros,
                TotalRegistros = totalRegistros,
                Datos = movimientos
            };
        }

        public async Task<bool> RegistrarMovimientoCompraAsync(int idProducto, int cantidad, int idCompra, string? nota = null)
        {
            // Validar existencia del producto
            var producto = await _context.productos.FindAsync(idProducto);
            if (producto == null)
            {
                return false;
            }

            // Crear el movimiento de inventario
            var movimiento = new MovimientosInventario
            {
                IdProducto = idProducto,
                TipoMovimiento = TipoMovimiento.entrada,
                Cantidad = cantidad,
                TipoDocumento = TipoDocumento.compra,
                IdDocumento = idCompra,
                Notas = nota
            };

            // Guardar movimiento
            await _context.movimientosInventario.AddAsync(movimiento);
            await _context.SaveChangesAsync();

            return true;
        }
        //RegistrarMovimientoPedidoAsync
        public async Task<bool> RegistrarMovimientoPedidoAsync(int idProducto, int cantidad, int idPedido, string? nota = null)
        {
            // Validar existencia del producto
            var producto = await _context.productos.FindAsync(idProducto);
            if (producto == null)
            {
                return false;
            }

            // Crear el movimiento de inventario
            var movimiento = new MovimientosInventario
            {
                IdProducto = idProducto,
                TipoMovimiento = TipoMovimiento.salida,
                Cantidad = cantidad,
                TipoDocumento = TipoDocumento.pedido,
                IdDocumento = idPedido,
                Notas = nota  
            };

            // Guardar movimiento
            await _context.movimientosInventario.AddAsync(movimiento);
            await _context.SaveChangesAsync();

            return true;
        }

               
    }
}