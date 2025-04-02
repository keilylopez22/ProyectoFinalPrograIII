using System;
using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Id_Proveedor { get; set; } // Clave foránea
        // Otras propiedades de la compra

        public Proveedor Proveedor { get; set; } // Propiedad de navegación
        public ICollection<DetalleCompra> DetallesCompra { get; set; }
    }
}