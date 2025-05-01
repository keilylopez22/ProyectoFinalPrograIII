using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
namespace ApiECommerce.Servicio// Para tus modelos (asegúrate de que el namespace sea correcto)

{
    public interface IComprasServicio
    {
        //metodos para obtener la informacion de la base de datos
        Task<IEnumerable<Compra>> ObtenerComprasAsync();
        Task<Compra> ObtenerComprasAsync(int id);
        Task<bool> CrearComprasAsync(Compra compra);
        Task<bool> ActualizarComprasAsync(Compra compra);
        Task<bool> EliminarComprasAsync(int id);


        Task<IEnumerable<Compra>> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProductos = null,
            int? IdProveedor = null);
        
    }


    public class CompraServicio:IComprasServicio
    {
         private readonly ApplicationDbContext _context;
         public CompraServicio(ApplicationDbContext context)
         {
            _context = context;

         }
        public async Task<IEnumerable<Compra>> ObtenerComprasAsync()
        {

            return await _context.compras.
            Include(p => p.Proveedor).
            Include(p  => p.DetalleCompras).
            ToListAsync();
        }

        //filtros
        public async Task<IEnumerable<Compra>> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProductos = null,
            int? IdProveedor = null)
        {
            var query = _context.compras
                .Include(c => c.Proveedor)
                .Include(c => c.DetalleCompras)
                    .ThenInclude(dc => dc.Producto) // Asumiendo que DetalleCompra tiene navegación a Producto
                .AsQueryable();

            // Filtro por rango de fechas
            if (fechaInicio.HasValue)
                query = query.Where(c => c.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(c => c.Fecha <= fechaFin.Value);

            // Filtro por proveedor
            if (IdProveedor.HasValue)
                query = query.Where(c => c.IdProveedor == IdProveedor.Value);

            // Filtro por producto
            if (IdProductos.HasValue)
                query = query.Where(c => c.DetalleCompras.Any(dc => dc.IdProductos == IdProductos.Value));

            return await query.ToListAsync();
        }

        public async Task<Compra> ObtenerComprasAsync(int id)
        {
            
            return await _context.compras.
            Include(p => p.Proveedor).
            Include(p  => p.DetalleCompras).
            FirstOrDefaultAsync(p => p.Id == id );
            
        }
        public async Task<bool> CrearComprasAsync(Compra compra)
        {
            if(compra == null)
            {
                return false;
            }

            await _context.compras.AddAsync(compra);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> ActualizarComprasAsync(Compra compra)
        {
            _context.compras.Update(compra);
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> EliminarComprasAsync(int id)
        {
            var compra = await _context.compras.FindAsync(id);

            if ( compra != null)
            {
                _context.compras.Remove(compra);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

    }
}