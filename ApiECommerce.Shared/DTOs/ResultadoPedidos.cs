using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    /// <summary>
    /// Resultado de la consulta de pedidos paginados.
    /// </summary>
    public class ResultadoPedidos
    {
        public List<Pedido> Pedidos { get; set; } = new();
        public int Total { get; set; }

    }
}