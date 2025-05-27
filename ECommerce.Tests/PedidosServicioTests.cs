using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.Servicio;
using ApiECommerce.Modelo;
using ApiECommerce.DTOs;
using ApiECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class PedidoServicioTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IMovimientosInventarioServicio> _movimientoMock;
    private readonly Mock<IKafkaProductorServicio> _kafkaMock;
    private readonly PedidoServicio _servicio;

    public PedidoServicioTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB única para cada test
            .Options;

        _context = new ApplicationDbContext(options);
        _movimientoMock = new Mock<IMovimientosInventarioServicio>();
        _kafkaMock = new Mock<IKafkaProductorServicio>();
        _servicio = new PedidoServicio(_context, _movimientoMock.Object, _kafkaMock.Object);
    }

    [Fact]
    public async Task CrearPedidosAsync_DeberiaCrearPedidoYDisminuirStock()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, CorreoElectronico = "cliente@correo.com", Nombre = "Cliente Test" , Direccion = "Calle Falsa 123" };
        var producto = new Producto { Id = 1, Nombre = "Producto1", Existencias = 10, Precio = 50m };

        await _context.clientes.AddAsync(cliente);
        await _context.productos.AddAsync(producto);
        await _context.SaveChangesAsync();

        var pedidoDto = new PedidoDTO
        {
            Fecha = DateTime.Now,
            IdCliente = cliente.Id,
            DetallesPedido = new List<DetallePedidoDto>
            {
                new DetallePedidoDto { IdProductos = producto.Id, CantidadProductos = 2 }
            }
        };

        // Act
        var resultado = await _servicio.CrearPedidosAsync(pedidoDto);

        // Assert
        Assert.True(resultado.Exito);
        Assert.NotNull(resultado.Pedido);
        Assert.Equal(1, await _context.pedidos.CountAsync());
        Assert.Equal(8, (await _context.productos.FindAsync(producto.Id)).Existencias);
    }

    [Fact]
    public async Task CrearPedidosAsync_DeberiaRetornarError_ProductoNoExiste()
    {
        var pedidoDto = new PedidoDTO
        {
            Fecha = DateTime.Now,
            IdCliente = 1,
            DetallesPedido = new List<DetallePedidoDto>
            {
                new DetallePedidoDto { IdProductos = 99, CantidadProductos = 1 }
            }
        };

        var resultado = await _servicio.CrearPedidosAsync(pedidoDto);

        Assert.False(resultado.Exito);
        Assert.Contains("no encontrado", resultado.Mensaje.ToLower());
    }

    [Fact]
    public async Task ObtenerPedidosAsync_DeberiaRetornarPedidoExistente()
    {
        var cliente = new Cliente { Id = 1, CorreoElectronico = "cliente@correo.com", Nombre = "Cliente Test" , Direccion = "Calle Falsa 123" };
        var producto = new Producto { Id = 1, Nombre = "Producto1", Existencias = 10, Precio = 50m };

        await _context.clientes.AddAsync(cliente);
        await _context.productos.AddAsync(producto);
        await _context.SaveChangesAsync();
        var pedido = new Pedido
        {
            Id = 123,
            Fecha = DateTime.Now,
            IdCliente = 1,
            Total = 100,
            Estado = "Pendiente",
            DetallesPedido = new List<DetallePedido>()
        };
        await _context.pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        var resultado = await _servicio.ObtenerPedidosAsync(123);

        Assert.NotNull(resultado);
        Assert.Equal(123, resultado.Id);
    }

    [Fact]
    public async Task EliminarPedidosAsync_DeberiaEliminarPedido()
    {
        var pedido = new Pedido { Id = 321, Fecha = DateTime.Now , IdCliente = 1, Total = 200, Estado = "Pendiente" };
        await _context.pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        var resultado = await _servicio.EliminarPedidosAsync(321);

        Assert.True(resultado);
        Assert.Null(await _context.pedidos.FindAsync(321));
    }

    [Fact]
    public async Task CambiarEstadoPedidoAsync_DeberiaActualizarEstadoCorrectamente()
    {
        var cliente = new Cliente { Id = 1, CorreoElectronico = "test@correo.com", Nombre = "Cliente Test", Direccion = "Calle Falsa 123" };
        var pedido = new Pedido { Id = 500, Estado = "Pendiente", Cliente = cliente, IdCliente = 1 };

        await _context.clientes.AddAsync(cliente);
        await _context.pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        var resultado = await _servicio.CambiarEstadoPedidoAsync(500, "Enviado");

        Assert.True(resultado.Exitoso);
        var pedidoActualizado = await _context.pedidos.FindAsync(500);
        Assert.Equal("Enviado", pedidoActualizado.Estado);
    }

    [Fact]
    public async Task CambiarEstadoPedidoAsync_DeberiaRechazarEstadoInvalido()
    {
        var cliente = new Cliente { Id = 2, CorreoElectronico = "correo@correo.com", Nombre = "Cliente Invalido", Direccion = "Calle Inexistente 456" };
        var pedido = new Pedido { Id = 777, Estado = "Pendiente", Cliente = cliente, IdCliente = 2 };

        await _context.clientes.AddAsync(cliente);
        await _context.pedidos.AddAsync(pedido);
        await _context.SaveChangesAsync();

        var resultado = await _servicio.CambiarEstadoPedidoAsync(777, "Desconocido");

        Assert.False(resultado.Exitoso);
        Assert.Contains("no es válido", resultado.Mensaje);
    }
}
