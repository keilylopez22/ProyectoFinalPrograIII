using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiECommerce.Modelo
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [ForeignKey("Pedido")]
        public int IdPedidos { get; set; }

        [ForeignKey("Producto")]
        public int IdProductos { get; set; }
        public decimal SubTotal { get; set; }

        public int CantidadProductos { get; set; }
        public decimal PrecioUnitario { get; set; }
         [JsonIgnore]

        public Pedido Pedido { get; set; }
        public Producto Producto { get; set; }
    }
}
