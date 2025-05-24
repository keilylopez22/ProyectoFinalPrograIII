using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ApiECommerce.Servicio;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ApiECommerce.DTOs;

public class CompraServicioTests
{
    private ApplicationDbContext CrearContextoInMemory(string nombreBD)
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: nombreBD)
            .Options;

        return new ApplicationDbContext(opciones);
    }

    private void SembrarDatos(ApplicationDbContext contexto)
    {
        contexto.proveedores.Add(new Proveedor { Id = 1, Nombre = "Proveedor A" });

        contexto.productos.Add(new Producto
        {
            Id = 1,
            Nombre = "Producto A",
            Existencias = 10,
            Precio = 100
        });

        contexto.compras.Add(new Compra
        {
            Id = 1,
            Fecha = DateTime.Today,
            IdProveedor = 1,
            Proveedor = contexto.proveedores.Find(1),
            DetalleCompras = new List<DetalleCompra>
            {
                new DetalleCompra
                {
                    Id = 1,
                    IdProductos = 1,
                    CantidadProductos = 2,
                    PrecioUnitario = 50
                }
            }
        });

        contexto.SaveChanges();
    }

    [Fact]
    public async Task CrearComprasAsync_DebeCrearCompraCorrectamente()
    {
        var dbName = Guid.NewGuid().ToString();
        using var contexto = CrearContextoInMemory(dbName);
        SembrarDatos(contexto);

        var mockMov = new Mock<IMovimientosInventarioServicio>();

        var servicio = new CompraServicio(contexto, mockMov.Object);

        var nuevaCompra = new CompraDTO
        {
            Fecha = DateTime.Today,
            IdProveedor = 1,
            DetalleCompras = new List<DetalleCompraDto>
            {
                new DetalleCompraDto
                {
                    IdProductos = 1,
                    CantidadProductos = 5,
                    PrecioUnitario = 45
                }
            }
        };

        var resultado = await servicio.CrearComprasAsync(nuevaCompra);

        Assert.True(resultado.Exito);
        Assert.Equal(2, contexto.compras.Count()); // Una de sembrado + una nueva
        Assert.Equal(15, contexto.productos.Find(1).Existencias); // 10 + 5
    }

    [Fact]
    public async Task EditarComprasAsync_DebeActualizarCompra()
    {
        var dbName = Guid.NewGuid().ToString();
        using var contexto = CrearContextoInMemory(dbName);
        SembrarDatos(contexto);

        var mockMov = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(contexto, mockMov.Object);

        var editarCompra = new Compra
        {
            Id = 1,
            Fecha = DateTime.Today.AddDays(-1),
            IdProveedor = 1,
            DetalleCompras = new List<DetalleCompra>
            {
                new DetalleCompra
                {
                    IdProductos = 1,
                    CantidadProductos = 3,
                    PrecioUnitario = 60
                }
            }
        };

        var resultado = await servicio.ActualizarComprasAsync(editarCompra);

        Assert.True(resultado);
        var compraEditada = contexto.compras.Include(c => c.DetalleCompras).First(c => c.Id == 1);
        Assert.Single(compraEditada.DetalleCompras);
        Assert.Equal(3, compraEditada.DetalleCompras.First().CantidadProductos);
    }

    [Fact]
    public async Task EliminarComprasAsync_DebeEliminarCompra()
    {
        var dbName = Guid.NewGuid().ToString();
        using var contexto = CrearContextoInMemory(dbName);
        SembrarDatos(contexto);

        var mockMov = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(contexto, mockMov.Object);

        var resultado = await servicio.EliminarComprasAsync(1);

        Assert.True(resultado);
        Assert.Empty(contexto.compras.ToList());
    }

    [Fact]
    public async Task ObtenerTodasComprasAsync_DebeRetornarLista()
    {
        var dbName = Guid.NewGuid().ToString();
        using var contexto = CrearContextoInMemory(dbName);
        SembrarDatos(contexto);

        var mockMov = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(contexto, mockMov.Object);


        var compras = await servicio.ObtenerComprasAsync(null, null, null);
        Assert.Single(compras);
        //Assert.Equal(1, compras[0].Id);
    }

    [Fact]
    public async Task ObtenerComprasPorIdAsync_DebeRetornarCompraCorrecta()
    {
        var dbName = Guid.NewGuid().ToString();
        using var contexto = CrearContextoInMemory(dbName);
        SembrarDatos(contexto);

        var mockMov = new Mock<IMovimientosInventarioServicio>();
        var servicio = new CompraServicio(contexto, mockMov.Object);

        var compra = await servicio.ObtenerComprasAsync(1);

        Assert.NotNull(compra);
        Assert.Equal(1, compra.Id);
    }
}
