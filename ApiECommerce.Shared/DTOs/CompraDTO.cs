using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    public class CompraDTO
    {
        public DateTime Fecha { get; set; }
        public int IdProveedor { get; set; }
        public List<DetalleCompraDto> DetalleCompras { get; set; }
    }

    public class DetalleCompraDto
    {
        public int IdProductos { get; set; }
        public int CantidadProductos { get; set; }
        public double PrecioUnitario { get; set; } // Agregado para el precio de compra
    }

    public class CompraResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public Compra Datos { get; set; } // Puedes cambiar a un DTO de respuesta si quieres
    }
            
    
}