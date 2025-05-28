using Xunit;
using Microsoft.EntityFrameworkCore;
using ApiECommerce.Servicio;
using ApiECommerce.Modelo;
using ApiECommerce.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ApiECommerce.Tests
{
    public class ClienteServicioTests
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
            var clientes = new List<Cliente>
            {
                new Cliente { Nombre = "Ana López" , Direccion = "Calle 123", Nit= 1546446, CorreoElectronico= "AnaPrueba@gmail.com", Telefono = 1234567890},
                new Cliente { Nombre = "Mario Pérez", Direccion = "Calle 456", Nit= 1546446, CorreoElectronico= "nkjnfkfkkmclmer", Telefono = 1234567890},
                new Cliente { Nombre = "María García", Direccion = "Calle 789", Nit= 1546446, CorreoElectronico= "nfefefjoer", Telefono = 1234567890},
                new Cliente { Nombre = "Juan Mendoza", Direccion = "Calle 101", Nit= 1546446, CorreoElectronico= "hfiefijero", Telefono = 1234567890},
                new Cliente { Nombre = "Luisa Fernández", Direccion = "Calle 202", Nit= 1546446, CorreoElectronico= "efejfjoiqf", Telefono = 1234567890},     
            };

            context.clientes.AddRange(clientes);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ObtenerClientesAsync_DeberiaRetornarTodos()
        {
            //inicializar el contexto de la base de datos en memoria
            var context = GetDbContext();
            //sembrar datos en la base de datos
            await SembrarDatos(context);
            //crear el servicio de cliente
            var servicio = new ClienteServicio(context);
            //obtener todos los clientes invocamos el metodo que vamos a probar
            var resultado = await servicio.ObtenerClientesAsync();
            //verificamos  el total de clientes en  el resultado 
            Assert.Equal(5, resultado.Clientes.Count);
            Assert.Equal(5, resultado.Total);
        }

        [Fact]
        public async Task ObtenerClientesAsync_ConFiltroNombre()
        {
            var context = GetDbContext();
            await SembrarDatos(context);

            var servicio = new ClienteServicio(context);
            var resultado = await servicio.ObtenerClientesAsync(nombre: "Mar");

            Assert.Equal(2, resultado.Clientes.Count); // Mario y María
        }

        [Fact]
        public async Task ObtenerClienteAsync_PorId()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var cliente = context.clientes.First();

            var servicio = new ClienteServicio(context);
            var resultado = await servicio.ObtenerClienteAsync(cliente.Id);

            Assert.NotNull(resultado);
            Assert.Equal(cliente.Nombre, resultado.Nombre);
        }

        [Fact]
        public async Task CrearClienteAsync_DeberiaAgregarCliente()
        {
            var context = GetDbContext();
            var servicio = new ClienteServicio(context);

            var nuevo = new Cliente { Nombre = "Nuevo Cliente" , Direccion = "Calle Nueva", Nit= 1546446, CorreoElectronico= "cjchiquwhefih", Telefono = 1234567890};
            var resultado = await servicio.CrearClienteAsync(nuevo);

            Assert.True(resultado);
            Assert.Single(context.clientes);
        }

        [Fact]
        public async Task ActualizarClienteAsync_DeberiaModificarCliente()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var cliente = context.clientes.First();
            cliente.Nombre = "Nombre Actualizado";

            var servicio = new ClienteServicio(context);
            var resultado = await servicio.ActualizarClienteAsync(cliente);

            Assert.True(resultado);
            Assert.Equal("Nombre Actualizado", context.clientes.First().Nombre);
        }

        [Fact]
        public async Task EliminarClienteAsync_DeberiaEliminarCliente()
        {
            var context = GetDbContext();
            await SembrarDatos(context);
            var cliente = context.clientes.First();

            var servicio = new ClienteServicio(context);
            var resultado = await servicio.EliminarClienteAsync(cliente.Id);

            Assert.True(resultado);
            Assert.Equal(4, context.clientes.Count());
        }
    }
}
