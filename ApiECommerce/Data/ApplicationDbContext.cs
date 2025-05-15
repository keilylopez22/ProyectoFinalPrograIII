using Microsoft.EntityFrameworkCore;
using ApiECommerce.Modelo; // Asegúrate de que la ruta sea correcta

namespace ApiECommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Proveedor> proveedores { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Compra> compras { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<DetallePedido> detallePedido { get; set; }
        public DbSet<DetalleCompra> detalleCompras { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<MovimientosInventario> movimientosInventario { get; set; }
       


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones (Fluent API)
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany()
                .HasForeignKey(c => c.IdProveedor);
            /*
            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Producto)
                .WithMany(pr => pr.DetallesPedido)
                .HasForeignKey(dp => dp.Id_Productos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Pedido)
                .WithMany(pe => pe.DetallesPedido)
                .HasForeignKey(dp => dp.Id_Pedidos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Producto)
                .WithMany(pr => pr.DetallesCompra)
                .HasForeignKey(dc => dc.IdProductos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE
            
            modelBuilder.Entity<DetalleCompra>()
                //.HasOne(dc => dc.Compra)
                //.WithMany()
                .HasForeignKey(dc => dc.IdCompras)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE
                */

                /*modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompras); // Aquí va el nombre real de la columna en la DB


                

                modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.IdProductos);*/
            modelBuilder.Entity<MovimientosInventario>()
                .Property(m => m.TipoDocumento)
                .HasConversion<string>();

            modelBuilder.Entity<MovimientosInventario>()
                .Property(m => m.TipoMovimiento)
                .HasConversion<string>();


        }
    }
}