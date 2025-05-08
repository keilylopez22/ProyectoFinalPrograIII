using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    public class PedidoResultado
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public Pedido? Pedido { get; set; }
    }

}