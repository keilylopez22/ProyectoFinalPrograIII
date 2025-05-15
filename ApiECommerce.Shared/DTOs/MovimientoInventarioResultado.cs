using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    public class MovimientoInventarioResultado
    {
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public List<MovimientosInventario> Datos { get; set; } = new List<MovimientosInventario>();
    }
 
}
