using System.Collections.Generic;

namespace ApiECommerce.Modelo
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int Nit { get; set; }
        public string CorreoElectronico { get; set; }
        public int Telefono { get; set; }

    }
}