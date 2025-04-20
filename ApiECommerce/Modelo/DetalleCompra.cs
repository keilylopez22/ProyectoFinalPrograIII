using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetalleCompra
    {
        public int Id { get; set; }

        [ForeignKey("Compra")]
        public int IdCompras { get; set; }

        [ForeignKey("Producto")]
        public int IdProductos { get; set; }

        public int CantidadProductos { get; set; }
        public decimal PrecioUnitario { get; set; }
        [JsonIgnore]
        public Compra Compra { get; set; }
        public Producto Producto { get; set; }
    }
}
