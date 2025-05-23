using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ApiECommerce.Modelo;
using ApiECommerce.Servicio;
using ApiECommerce.Data;
using ApiECommerce.DTOs;

namespace ApiECommerce.Tests
{
    public class ProductoServicioTests
    {
        //inicializa el contexto de la base de datos en memoria
        private ApplicationDbContext ObtenerContextoEnMemoria()
        {
            // Configura el contexto de la base de datos en memoria
            // para que use un nombre de base de datos único por prueba
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(opciones);
        }

        private async Task SembrarDatos(ApplicationDbContext context)
        {
            context.productos.AddRange(
                new Producto { Id = 1, Nombre = "Producto A", Precio = 10, Existencias = 5, IdCategoria = 1 },
                new Producto { Id = 2, Nombre = "Producto B", Precio = 20, Existencias = 2, IdCategoria = 2 }
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ObtenerProductosAsync_RetornaProductosPaginados()
        {
            // Arrange
            var context = ObtenerContextoEnMemoria();
            await SembrarDatos(context);
            var servicio = new ProductoServicio(context);

            // Act
            var resultado = await servicio.ObtenerProductosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.TotalElementos);
            Assert.Equal(2, resultado.Productos.Count);
        }

        [Fact]
        public async Task ObtenerProductoPorIdAsync_ProductoExiste_RetornaProducto()
        {
            var context = ObtenerContextoEnMemoria();
            await SembrarDatos(context);
            var servicio = new ProductoServicio(context);

            var producto = await servicio.ObtenerProductoPorIdAsync(1);

            Assert.NotNull(producto);
            Assert.Equal("Producto A", producto.Nombre);
        }

        [Fact]
        public async Task CrearProductosAsync_CreaProductoCorrectamente()
        {
            var context = ObtenerContextoEnMemoria();
            var servicio = new ProductoServicio(context);

            var producto = new Producto { Id = 3, Nombre = "Producto C", Precio = 30, Existencias = 10 };

            var resultado = await servicio.CrearProductosAsync(producto);

            Assert.True(resultado);
            Assert.Equal(1, await context.productos.CountAsync());
        }

        [Fact]
        public async Task ActualizarProductosAsync_ProductoExiste_ActualizaCorrectamente()
        {
            var context = ObtenerContextoEnMemoria();
            await SembrarDatos(context);
            var servicio = new ProductoServicio(context);

            var producto = await context.productos.FindAsync(1);
            producto.Nombre = "Producto A Modificado";

            var resultado = await servicio.ActualizarProductosAsync(producto);

            Assert.True(resultado);
            var actualizado = await context.productos.FindAsync(1);
            Assert.Equal("Producto A Modificado", actualizado.Nombre);
        }

        [Fact]
        public async Task EliminarProductosAsync_ProductoExiste_EliminaCorrectamente()
        {
            var context = ObtenerContextoEnMemoria();
            await SembrarDatos(context);
            var servicio = new ProductoServicio(context);

            var resultado = await servicio.EliminarProductosAsync(1);

            Assert.True(resultado);
            Assert.Null(await context.productos.FindAsync(1));
        }

        [Fact]
        public async Task EliminarProductosAsync_ProductoNoExiste_RetornaFalse()
        {
            var context = ObtenerContextoEnMemoria();
            var servicio = new ProductoServicio(context);

            var resultado = await servicio.EliminarProductosAsync(999);

            Assert.False(resultado);
        }
    }
}
