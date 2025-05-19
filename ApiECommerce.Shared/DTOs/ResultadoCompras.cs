using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;

namespace ApiECommerce.DTOs
{
    public class ResultadoCompras
    {
        public List<Compra> Compras { get; set; } 
        public int Total { get; set; } 
    }
}