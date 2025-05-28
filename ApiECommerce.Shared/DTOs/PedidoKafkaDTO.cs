using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class PedidoKafkaDTO
    {
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleKafkaDTO> DetallesPedido { get; set; } = new();
    }

    public class DetalleKafkaDTO
    {
        public int IdProductos { get; set; }
        public int CantidadProductos { get; set; }
    }    
}