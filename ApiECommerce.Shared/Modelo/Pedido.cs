using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiECommerce.Modelo
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }

        public decimal Total { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }

        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
