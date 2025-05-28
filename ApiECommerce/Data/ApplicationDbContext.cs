using Microsoft.EntityFrameworkCore;
using ApiECommerce.Modelo; 

namespace ApiECommerce.Data
{
    /// <summary>
    /// Contexto de base de datos principal para la aplicación de E-Commerce.
    /// Hereda de DbContext y gestiona el acceso a las entidades del dominio.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones del contexto desde la configuración externa.
        /// </summary>
        /// <param name="options">Opciones para configurar el contexto.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets que representan las tablas en la base de datos

        public DbSet<Cliente> clientes { get; set; } // Tabla de clientes
        public DbSet<Proveedor> proveedores { get; set; } // Tabla de proveedores
        public DbSet<Producto> productos { get; set; } // Tabla de productos
        public DbSet<Compra> compras { get; set; } // Tabla de compras
        public DbSet<Pedido> pedidos { get; set; } // Tabla de pedidos
        public DbSet<DetallePedido> detallePedido { get; set; } // Tabla de detalles de pedidos
        public DbSet<DetalleCompra> detalleCompras { get; set; } // Tabla de detalles de compras
        public DbSet<Categoria> categorias { get; set; } // Tabla de categorías de productos
        public DbSet<MovimientosInventario> movimientosInventario { get; set; } // Tabla de movimientos de inventario

        /// <summary>
        /// Configuración personalizada de las entidades y relaciones usando Fluent API.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación: una compra tiene un proveedor (uno a muchos)
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany()
                .HasForeignKey(c => c.IdProveedor);

            // Conversión de enumeraciones a string en la base de datos
            modelBuilder.Entity<MovimientosInventario>()
                .Property(m => m.TipoDocumento)
                .HasConversion<string>();

            modelBuilder.Entity<MovimientosInventario>()
                .Property(m => m.TipoMovimiento)
                .HasConversion<string>();
        }
    }
}
