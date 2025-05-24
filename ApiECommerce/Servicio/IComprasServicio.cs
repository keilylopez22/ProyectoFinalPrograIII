using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; // Para DbContext, DbSet, etc.
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.DTOs;
namespace ApiECommerce.Servicio// Para tus modelos (asegúrate de que el namespace sea correcto)


{
    public interface IComprasServicio
    {
        //metodos para obtener la informacion de la base de datos
        Task<IEnumerable<Compra>> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProveedor = null);
        Task<Compra> ObtenerComprasAsync(int id);
        Task<CompraResultado> CrearComprasAsync(CompraDTO compraDTO);
        Task<bool> ActualizarComprasAsync(Compra compra);
        Task<bool> EliminarComprasAsync(int id);


        Task<ResultadoCompras> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProveedor = null,
            int pageNumber = 1,
            int pageSize = 10

            );
        
    }


    public class CompraServicio:IComprasServicio
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovimientosInventarioServicio _movimientoInventarioServicio;
         public CompraServicio(ApplicationDbContext context, IMovimientosInventarioServicio movimientoInventarioServicio)
         {
            _movimientoInventarioServicio = movimientoInventarioServicio;
            _context = context;

         }
        public async Task<IEnumerable<Compra>> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProveedor = null
        )
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
            
            var compras = await query
               .ToListAsync();

            return compras;
            
        }

        //filtros
        public async Task<ResultadoCompras> ObtenerComprasAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProveedor = null,
            int pageNumber = 1,
            int pageSize = 10
            )

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
            var total = query.Count();
             var compras = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var resultado = new ResultadoCompras
            {
                Compras = compras,
                Total = total
            };
            return resultado;
        }

        public async Task<Compra> ObtenerComprasAsync(int id)
        {
            
            return await _context.compras.
            Include(p => p.Proveedor).
            Include(p  => p.DetalleCompras).
            FirstOrDefaultAsync(p => p.Id == id );
            
        }
       /* public async Task<bool> CrearComprasAsync(Compra compra)
        {
            if(compra == null)
            {
                return false;
            }

            await _context.compras.AddAsync(compra);
            await _context.SaveChangesAsync();
            return true;

        }
        */
        public async Task<CompraResultado> CrearComprasAsync(CompraDTO compraDto)
        {
            if (compraDto == null || compraDto.DetalleCompras == null || !compraDto.DetalleCompras.Any())
            {
                return new CompraResultado { Exito = false, Mensaje = "La compra está vacía o es inválida." };
            }

            double totalCompra = 0;
            var detallesCompra = new List<DetalleCompra>();

            foreach (var item in compraDto.DetalleCompras)
            {
                var producto = await _context.productos.FindAsync(item.IdProductos);

                if (producto == null)
                {
                    return new CompraResultado { Exito = false, Mensaje = $"Producto con ID {item.IdProductos} no encontrado." };
                }

                // Sumar existencias al producto
                producto.Existencias += item.CantidadProductos;
                //acrualizar el precio del producto
                producto.Precio = (decimal)item.PrecioUnitario;

                double precioUnitario = item.PrecioUnitario;
                double subTotal = precioUnitario * item.CantidadProductos;
                totalCompra += subTotal;

                detallesCompra.Add(new DetalleCompra
                {
                    IdProductos = item.IdProductos,
                    CantidadProductos = item.CantidadProductos,
                    PrecioUnitario = (decimal)precioUnitario,
                    SubTotal = (decimal)subTotal
                });
            }

            var compra = new Compra
            {
                Fecha = compraDto.Fecha,
                IdProveedor = compraDto.IdProveedor,
                Estado = "Completada",
                Total = totalCompra,
                DetalleCompras = detallesCompra
            };

            await _context.compras.AddAsync(compra);
            await _context.SaveChangesAsync();

            // Registrar los movimientos de inventario tipo compra
            foreach (var detalle in compra.DetalleCompras)
            {                await _movimientoInventarioServicio.RegistrarMovimientoCompraAsync(
                    detalle.IdProductos,
                    detalle.CantidadProductos,
                    compra.Id,
                    $"Compra registrada el {DateTime.Now}",
                    detalle.PrecioUnitario
                );
            }

            return new CompraResultado
            {
                Exito = true,
                Mensaje = "Compra registrada exitosamente.",
                Datos = compra
            };
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