using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Existencias { get; set; }
        public int? IdCategoria { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Descripcion { get; set; } 
    }
}