using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class PedidoEventoDTO
    {
        public string Evento { get; set; }              
        public int PedidoId { get; set; }
        public string Estado { get; set; }              
        public string EstadoAnterior { get; set; }     
        public string EstadoNuevo { get; set; }         
        public string ClienteEmail { get; set; }
    }   
}