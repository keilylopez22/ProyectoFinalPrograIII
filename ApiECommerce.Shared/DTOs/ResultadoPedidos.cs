using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    public class ResultadoPedidos
    {
        public List<Pedido> Pedidos { get; set; } = new();
        public int TotalRegistros { get; set; }
       
    }
}