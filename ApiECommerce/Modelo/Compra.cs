using System;
using System.Collections.Generic;

namespace ApiECommerce.Modelo
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdProveedor { get; set; } // Clave foránea
        // Otras propiedades de la compra

        public double Total {get; set;}

        public string Estado {get; set;}
        public Proveedor Proveedor { get; set; } // Propiedad de navegación
        public ICollection<DetalleCompra> DetalleCompras { get; set; }
    }
}