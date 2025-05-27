using Xunit;
using Moq;
using System.Threading.Tasks;
using ApiECommerce.Servicio;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ApiECommerce.Tests
{
    public class MovimientosInventarioServicioTests
    {
        private async Task<ApplicationDbContext> GetDbContextWithDataAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Aisla cada prueba
                .Options;

            var context = new ApplicationDbContext(options);

            // Datos iniciales
            var producto = new Producto { Id = 1, Nombre = "Producto de prueba" };
            await context.productos.AddAsync(producto);
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task RegistrarMovimientoCompraAsync_ProductoExiste_RetornaTrue()
        {
            var context = await GetDbContextWithDataAsync();
            var servicio = new MovimientosInventarioServicio(context);

            var resultado = await servicio.RegistrarMovimientoCompraAsync(1, 10, 101, "Compra test", 5.5m);

            Assert.True(resultado);
            Assert.Single(context.movimientosInventario);
            Assert.Equal(TipoMovimiento.entrada, context.movimientosInventario.FirstAsync().Result.TipoMovimiento);
        }

        [Fact]
        public async Task RegistrarMovimientoCompraAsync_ProductoNoExiste_RetornaFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb_NoExiste")
                .Options;

            var context = new ApplicationDbContext(options);
            var servicio = new MovimientosInventarioServicio(context);

            var resultado = await servicio.RegistrarMovimientoCompraAsync(999, 10, 101, "Compra inválida");

            Assert.False(resultado);
        }

        [Fact]
        public async Task RegistrarMovimientoPedidoAsync_ProductoExiste_RetornaTrue()
        {
            var context = await GetDbContextWithDataAsync();
            var servicio = new MovimientosInventarioServicio(context);

            var resultado = await servicio.RegistrarMovimientoPedidoAsync(1, 5, 202, "Pedido test", 6m);

            Assert.True(resultado);
            var movimiento = await context.movimientosInventario.FirstAsync();
            Assert.Equal(TipoMovimiento.salida, movimiento.TipoMovimiento);
            Assert.Equal(TipoDocumento.pedido, movimiento.TipoDocumento);
        }

        [Fact]
        public async Task ObtenerPedidosAsync_SinFiltros_RetornaTodos()
        {
            var context = await GetDbContextWithDataAsync();
            var servicio = new MovimientosInventarioServicio(context);

            // Agrega movimientos
            await servicio.RegistrarMovimientoCompraAsync(1, 5, 301, "Compra 1");
            await servicio.RegistrarMovimientoPedidoAsync(1, 2, 302, "Pedido 1");

            var resultado = await servicio.ObtenerPedidosAsync();

            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Equal(2, resultado.Datos.Count);
        }

        [Fact]
        public async Task ObtenerPedidosAsync_ConFiltroDeFecha_RetornaFiltrados()
        {
            var context = await GetDbContextWithDataAsync();
            var servicio = new MovimientosInventarioServicio(context);

            // Movimiento viejo
            var viejo = new MovimientosInventario
            {
                IdProducto = 1,
                TipoMovimiento = TipoMovimiento.entrada,
                Cantidad = 1,
                TipoDocumento = TipoDocumento.compra,
                IdDocumento = 100,
                FechaMovimiento = new DateTime(2023, 1, 1)
            };

            await context.movimientosInventario.AddAsync(viejo);
            await context.SaveChangesAsync();

            // Movimiento reciente
            await servicio.RegistrarMovimientoCompraAsync(1, 10, 200, "Compra reciente");

            var desde = new DateTime(2024, 1, 1);
            var resultado = await servicio.ObtenerPedidosAsync(fechaInicio: desde);

            Assert.Single(resultado.Datos);
        }
    }
}
