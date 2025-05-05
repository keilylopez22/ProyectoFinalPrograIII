using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class PedidoDTO
    {
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        
        public List<DetallePedidoDto> DetallesPedido { get; set; } = new();
    }

    public class DetallePedidoDto
    {
        public int IdProductos { get; set; }
        public int CantidadProductos { get; set; }
        
    }

}