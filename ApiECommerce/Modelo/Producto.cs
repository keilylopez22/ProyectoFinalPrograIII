using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        // Otras propiedades del producto

        public ICollection<DetallePedido> DetallesPedido { get; set; }
        public ICollection<DetalleCompra> DetallesCompra { get; set; }
    }
}