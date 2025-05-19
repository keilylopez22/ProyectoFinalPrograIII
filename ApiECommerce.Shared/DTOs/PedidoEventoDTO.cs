using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce.DTOs
{
    public class PedidoEventoDTO
    {
        public string Evento { get; set; }              // "pedido_creado" o "estado_actualizado"
        public int PedidoId { get; set; }
        public string Estado { get; set; }              // estado actual
        public string EstadoAnterior { get; set; }      // si aplica
        public string EstadoNuevo { get; set; }         // si aplica
        public string ClienteEmail { get; set; }
    }

   
}