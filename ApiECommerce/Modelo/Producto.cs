using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public  decimal Precio {get; set;}

        public int Existencias {get; set;}
        // Otras propiedades del producto

       
    }
}