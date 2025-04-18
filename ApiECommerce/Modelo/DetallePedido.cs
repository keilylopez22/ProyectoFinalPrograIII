namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdPedidos { get; set; } // Clave foránea
        public int IdProductos { get; set; } // Clave foránea
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Pedido Pedido { get; set; } // Propiedad de navegación
        public Producto Producto { get; set; } // Propiedad de navegación
    }
}