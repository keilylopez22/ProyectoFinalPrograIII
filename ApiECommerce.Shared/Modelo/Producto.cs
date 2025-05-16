using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiECommerce.Modelo
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public  decimal Precio {get; set;}

        public int Existencias {get; set;}

        public string? ImagenUrl { get; set; } // <-- Agrega esta línea si no existe
        public string? Descripcion { get; set; }
        

        [ForeignKey("Categoria")]
        public int? IdCategoria { get; set; }
        public Categoria Categoria { get; set; }
        // Otras propiedades del producto

       
    }
}