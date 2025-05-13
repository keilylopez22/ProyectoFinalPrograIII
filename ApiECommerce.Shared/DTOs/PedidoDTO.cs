using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

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

    public class PedidoResultado
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public Pedido? Pedido { get; set; }
    }

}