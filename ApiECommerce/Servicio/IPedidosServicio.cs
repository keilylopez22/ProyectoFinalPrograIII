using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using ApiECommerce.DTOs;

namespace ApiECommerce.Servicio
{
    public interface IPedidosServicio
    {
        Task<IEnumerable<Pedido>> ObtenerPedidosAsync();
        Task<Pedido> ObtenerPedidosAsync(int id);
        Task<bool> CrearPedidosAsync(Pedido pedido);
        Task<PedidoResultado?> CrearPedidosAsync(PedidoDTO pedidoDto);
        Task<bool> ActualizarPedidosAsync(PedidoUpdateDTO pedido);
        Task<bool> EliminarPedidosAsync(int id);
        

        

        Task<ResultadoPedidos> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdCliente = null,
            int? pageNumber = 1,
            int? pageSize = 10);

        Task<IEnumerable<Pedido>> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdCliente = null
        );


            //para cambiar el estado del pedido
        Task<ResultadoCambioEstado> CambiarEstadoPedidoAsync(int id, string nuevoEstado);
        
    }


    public class PedidoServicio:IPedidosServicio
    {
         private readonly ApplicationDbContext _context;
        private readonly IMovimientosInventarioServicio _movimientoInventarioServicio;
        private readonly IKafkaProductorServicio _kafkaProductorServicio;
        //Constructor
        public PedidoServicio(ApplicationDbContext context, IMovimientosInventarioServicio movimientoInventarioServicio, IKafkaProductorServicio kafkaProductorServicio)
        {
            _kafkaProductorServicio = kafkaProductorServicio;
            _movimientoInventarioServicio = movimientoInventarioServicio;
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
        public async Task<ResultadoPedidos> ObtenerPedidosAsync(
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int? IdCliente = null,
        int? pageNumber = 1,
        int? pageSize = 10)
        {
            var query = _context.pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(dp => dp.Producto)
                .AsQueryable();

            // Filtro por rango de fechas
            if (fechaInicio.HasValue)
                query = query.Where(p => p.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(p => p.Fecha <= fechaFin.Value);

            // Filtro por cliente
            if (IdCliente.HasValue)
                query = query.Where(p => p.IdCliente == IdCliente.Value);

            var total = query.Count();
            // Paginación
            int skip = ((pageNumber ?? 1) - 1) * (pageSize ?? 10);
            var pedidos = await query
                .Skip(skip)
                .Take(pageSize ?? 10)
                .ToListAsync();
            var resultado = new ResultadoPedidos
            {
                Pedidos = pedidos,
                Total = total
            };
            return resultado;
        }

        public async Task<IEnumerable<Pedido>> ObtenerPedidosAsync(
        DateTime? fechaInicio = null,
        DateTime? fechaFin = null,
        int? IdCliente = null
        )
        {
            var query = _context.pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(dp => dp.Producto)
                .AsQueryable();

            // Filtro por rango de fechas
            if (fechaInicio.HasValue)
                query = query.Where(p => p.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(p => p.Fecha <= fechaFin.Value);

            // Filtro por cliente
            if (IdCliente.HasValue)
                query = query.Where(p => p.IdCliente == IdCliente.Value);
            // Paginación
            var pedidos = await query
                .ToListAsync();
            return pedidos;
        }

        public async Task<Pedido> ObtenerPedidosAsync(int id)
        {

            return await _context.pedidos.
            Include(p => p.Cliente).
            Include(p => p.DetallesPedido).
            FirstOrDefaultAsync(p => p.Id == id);

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

        //Sobrecarga del método CrearPedidosAsync para recibir un PedidoDto

        public async Task<PedidoResultado> CrearPedidosAsync(PedidoDTO pedidoDto)
        {
            if (pedidoDto == null || pedidoDto.DetallesPedido == null || !pedidoDto.DetallesPedido.Any())
            {
                return new PedidoResultado { Exito = false, Mensaje = "El pedido está vacío o es inválido." };
            }

            decimal totalPedido = 0;
            var detallesPedido = new List<DetallePedido>();

            foreach (var item in pedidoDto.DetallesPedido)
            {
                var producto = await _context.productos.FindAsync(item.IdProductos);

                if (producto == null)
                {
                    return new PedidoResultado { Exito = false, Mensaje = $"Producto con ID {item.IdProductos} no encontrado." };
                }

                if (producto.Existencias < item.CantidadProductos)
                {
                    return new PedidoResultado { Exito = false, Mensaje = $"Stock insuficiente para el producto '{producto.Nombre}'." };
                }

                decimal precioUnitario = producto.Precio;

                producto.Existencias -= item.CantidadProductos;
                totalPedido += precioUnitario * item.CantidadProductos;

                detallesPedido.Add(new DetallePedido
                {
                    IdProductos = item.IdProductos,
                    CantidadProductos = item.CantidadProductos,
                    PrecioUnitario = precioUnitario,
                    SubTotal = precioUnitario * item.CantidadProductos
                });
            }

            var pedido = new Pedido
            {
                Fecha = pedidoDto.Fecha,
                IdCliente = pedidoDto.IdCliente,
                Cliente = await _context.clientes.FindAsync(pedidoDto.IdCliente),
                Estado = "Pendiente",
                Total = totalPedido,
                DetallesPedido = detallesPedido
            };

            await _context.pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();

             // Registrar los movimientos de inventario tipo compra
            foreach (var detalle in pedido.DetallesPedido)
            {
                await _movimientoInventarioServicio.RegistrarMovimientoPedidoAsync(
                    detalle.IdProductos,
                    detalle.CantidadProductos,
                    pedido.Id,
                    $"Pedido registrado el {DateTime.Now}",
                    detalle.PrecioUnitario
                );
            }

            // Enviar el pedido a Kafka
            var evento = new PedidoEventoDTO
            {
                Evento = "PedidoCreado",
                PedidoId = pedido.Id,
                Estado = pedido.Estado,
                ClienteEmail = pedido.Cliente.CorreoElectronico
            };

            await _kafkaProductorServicio.EnviarEventoAsync("pedidos-eventos", evento);

            return new PedidoResultado { Exito = true, Pedido = pedido };
        }

        public async Task<bool> ActualizarPedidosAsync(PedidoUpdateDTO pedido)
        {
            if (pedido == null || pedido.Id <= 0)
            {
                return false;
            }
            var pedidoExistente = await _context.pedidos
                .Include(p => p.DetallesPedido)
                .FirstOrDefaultAsync(p => p.Id == pedido.Id);
            if (pedidoExistente == null)
            {
                return false;
            }
            // Actualizar los campos del pedido
            pedidoExistente.Fecha = pedido.Fecha;
            pedidoExistente.IdCliente = pedido.IdCliente;
            pedidoExistente.DetallesPedido.Clear(); // Limpiar los detalles existentes
            foreach (var detalle in pedido.DetallesPedido)
            {
                var detalleExistente = new DetallePedido
                {
                    IdProductos = detalle.IdProductos,
                    CantidadProductos = detalle.CantidadProductos,
                    PrecioUnitario = 0, // Asignar un valor por defecto o calcularlo si es necesario
                    SubTotal = 0 // Asignar un valor por defecto o calcularlo si es necesario
                };
                pedidoExistente.DetallesPedido.Add(detalleExistente);
            }
            // Aquí podrías recalcular el total del pedido si es necesario
            pedidoExistente.Total = pedidoExistente.DetallesPedido.Sum(dp => dp.SubTotal);

            _context.pedidos.Update(pedidoExistente);
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
            var pedido = await _context.pedidos.Include(p => p.Cliente).FirstOrDefaultAsync(p => p.Id == id );
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

            // Enviar el evento a Kafka
            var evento = new PedidoEventoDTO
            {
                Evento = "EstadoPedidoCambiado",
                PedidoId = id,
                Estado = nuevoEstado,
                EstadoAnterior = estadoActual,
                EstadoNuevo = nuevoEstado,
                ClienteEmail = pedido.Cliente.CorreoElectronico
            };

            await _kafkaProductorServicio.EnviarEventoAsync("pedidos-eventos", evento);


            return new ResultadoCambioEstado
            {
                Exitoso = true,
                Mensaje = "Estado del pedido actualizado exitosamente."
            };
        }
    }
}