using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Data;
using ApiECommerce.Modelo;
using ApiECommerce.Servicio;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class CompraServicioTests
{
    private async Task<ApplicationDbContext> GetDbContextWithDataAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var proveedor = new Proveedor { Id = 1, Nombre = "Proveedor Test" };
        var producto = new Producto { Id = 1, Nombre = "Producto Test", Existencias = 10, Precio = 20 };

        context.proveedores.Add(proveedor);
        context.productos.Add(producto);

        var compra = new Compra
        {
            Id = 1,
            Fecha = DateTime.Now,
            IdProveedor = 1,
            Total = 200,
            Estado = "Completada",
            Proveedor = proveedor,
            DetalleCompras = new List<DetalleCompra>
            {
                new DetalleCompra
                {
                    Id = 1,
                    IdProductos = 1,
                    CantidadProductos = 5,
                    PrecioUnitario = 20,
                    SubTotal = 100
                }
            }
        };

        context.compras.Add(compra);

        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task ObtenerComprasAsync_RetornaTodasLasCompras()
    {
        var context = await GetDbContextWithDataAsync();
        var mockMovimiento = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(context, mockMovimiento.Object);

        var resultado = await servicio.ObtenerComprasAsync(
            fechaInicio: null,
            fechaFin: null,
            IdProveedor: null
        );

        Assert.Single(resultado);
        Assert.Equal(1, resultado.First().Id);
    }

    [Fact]
    public async Task ObtenerComprasAsyncPorId_RetornaCompraCorrecta()
    {
        var context = await GetDbContextWithDataAsync();
        var mockMovimiento = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(context, mockMovimiento.Object);

        var compra = await servicio.ObtenerComprasAsync(1);

        Assert.NotNull(compra);
        Assert.Equal(1, compra.Id);
        Assert.Equal("Completada", compra.Estado);
    }

    [Fact]
    public async Task EliminarComprasAsync_CompraExistente_EliminaCorrectamente()
    {
        var context = await GetDbContextWithDataAsync();
        var mockMovimiento = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(context, mockMovimiento.Object);

        var eliminado = await servicio.EliminarComprasAsync(1);

        Assert.True(eliminado);
        Assert.Empty(context.compras);
    }

    [Fact]
    public async Task EliminarComprasAsync_CompraNoExiste_RetornaFalse()
    {
        var context = await GetDbContextWithDataAsync();
        var mockMovimiento = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(context, mockMovimiento.Object);

        var eliminado = await servicio.EliminarComprasAsync(999);

        Assert.False(eliminado);
    }

    [Fact]
    public async Task ObtenerComprasConPaginacion_RetornaCorrecto()
    {
        var context = await GetDbContextWithDataAsync();
        var mockMovimiento = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(context, mockMovimiento.Object);

        var resultado = await servicio.ObtenerComprasAsync(
            fechaInicio: null,
            fechaFin: null,
            IdProveedor: null,
            pageNumber: 1,
            pageSize: 10
        );

        Assert.Single(resultado.Compras);
        Assert.Equal(1, resultado.Total);
    }
}
