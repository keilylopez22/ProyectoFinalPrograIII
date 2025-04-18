namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int IdCompras { get; set; } // Clave foránea
        public int IdProductos { get; set; } // Clave foránea
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Compra Compra { get; set; } // Propiedad de navegación
        public Producto Producto { get; set; } // Propiedad de navegación
    }
}