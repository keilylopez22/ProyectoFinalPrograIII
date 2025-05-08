using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class ProductoDTO
{
    public string Nombre { get; set; }
    public int Existencias { get; set; }
    public decimal Precio { get; set; }
    public int IdCategoria { get; set; }
}

}