using System;
using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; } // Clave foránea
        // Otras propiedades del pedido

        public Cliente Cliente { get; set; } // Propiedad de navegación
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}