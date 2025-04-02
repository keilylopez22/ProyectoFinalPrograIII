using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        // Otras propiedades del proveedor

        public ICollection<Compra> Compras { get; set; }
    }
}