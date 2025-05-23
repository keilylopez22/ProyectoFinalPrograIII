using Xunit;
using Microsoft.EntityFrameworkCore;
using ApiECommerce.Servicio;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ApiECommerce.Tests
{
    public class ProveedorServicioTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        private async Task SembrarDatos(ApplicationDbContext context)
        {
            var proveedores = new List<Proveedor>
            {
               new Proveedor { Nombre = "Suministros Globales" , Direccion = "Calle 123", Nit= 1546446, CorreoElectronico= "SuministrosGlobalesPrueba@gmail.com", Telefono = 1234567890},
                new Proveedor { Nombre = "Proveedores Express", Direccion = "Calle 456", Nit= 1546446, CorreoElectronico= "cbuawcuewfqf", Telefono = 1234567890},
                new Proveedor { Nombre = "Distribuciones Rápidas", Direccion = "Calle 789", Nit= 1546446, CorreoElectronico= "ijedwskckac", Telefono = 1234567890},
                new Proveedor { Nombre = "Logística Avanzada", Direccion = "Calle 101", Nit= 1546446, CorreoElectronico= "qwdxowojw", Telefono = 1234567890},
                new Proveedor { Nombre = "Servicios de Transporte", Direccion = "Calle 202", Nit= 1546446, CorreoElectronico= "hciaisfowaj", Telefono = 1234567890}
            };
        

            context.proveedores.AddRange(proveedores);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ObtenerProveedoresAsync_DeberiaRetornarTodos()
        {
            var context = GetDbContext();
            await SembrarDatos(context);

            var servicio = new ProveedorServicio(context);
            var resultado = await servicio.ObtenerProveedoresAsync();

            Assert.Equal(5, resultado.Proveedores.Count);
            Assert.Equal(5, resultado.Total);
        }

        [Fact]
        public async Task ObtenerProveedoresAsync_ConFiltroNombre()
        {
            var context = GetDbContext();
            await SembrarDatos(context);

            var servicio = new ProveedorServicio(context);
            var resultado = await servicio.ObtenerProveedoresAsync(nombre: "Pro");

            Assert.Equal(1, resultado.Proveedores.Count); // "Proveedores Express"
        }

        [Fact]
        public async Task ObtenerProveedorPorIdAsync_DeberiaRetornarProveedor()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var proveedor = context.proveedores.First();

            var servicio = new ProveedorServicio(context);
            var resultado = await servicio.ObtenerProveedorPorIdAsync(proveedor.Id);

            Assert.NotNull(resultado);
            Assert.Equal(proveedor.Nombre, resultado.Nombre);
        }

        [Fact]
        public async Task CrearProveedorAsync_DeberiaCrearNuevoProveedor()
        {
            var context = GetDbContext();
            var servicio = new ProveedorServicio(context);

            var nuevo = new Proveedor { Nombre = "Nuevo Proveedor" };
            var resultado = await servicio.CrearProveedorAsync(nuevo);

            Assert.True(resultado);
            Assert.Single(context.proveedores);
        }

        [Fact]
        public async Task ActualizarProveedorAsync_DeberiaActualizarProveedor()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var proveedor = context.proveedores.First();
            proveedor.Nombre = "Nombre Actualizado";

            var servicio = new ProveedorServicio(context);
            var resultado = await servicio.ActualizarProveedorAsync(proveedor);

            Assert.True(resultado);
            Assert.Equal("Nombre Actualizado", context.proveedores.First().Nombre);
        }

        [Fact]
        public async Task EliminarProveedorAsync_DeberiaEliminarProveedor()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var proveedor = context.proveedores.First();

            var servicio = new ProveedorServicio(context);
            var resultado = await servicio.EliminarProveedorAsync(proveedor.Id);

            Assert.True(resultado);
            Assert.Equal(4, context.proveedores.Count());
        }
    }
}
