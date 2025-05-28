namespace ApiECommerce.Modelo
{
    public class Proveedor
    {
        public int Id { get; set; }    
        public string Nombre { get; set; } =string.Empty;
        public string Direccion { get; set;}= string.Empty;       
        public int Nit{get; set;}
        public string CorreoElectronico{get; set; }= string.Empty;
        public int Telefono { get; set; }
    }
}