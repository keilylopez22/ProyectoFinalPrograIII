namespace ApiECommerce.Modelo
{
public class Proveedor
{
    public int Id { get; set; }
   
    public string Nombre { get; set; }/*chat*/ =string.Empty;

    public string Direccion { get; set;}/*chat*/ = string.Empty;
    
    public int Nit{get; set;}
    public string Correo_Electronico{get; set; }/*chat*/ = string.Empty;

    public int Telefono { get; set; }


    }
}