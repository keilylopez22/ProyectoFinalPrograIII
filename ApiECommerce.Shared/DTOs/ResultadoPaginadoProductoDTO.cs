using ApiECommerce.DTOs; 

namespace ApiECommerce.DTOs
{
    public class ResultadoPaginadoProductoDTO
    {
        public List<ProductoDTO> Productos { get; set; } = new();
        public int TotalElementos { get; set; }
        public int PaginaActual { get; set; }
        public int ElementosPorPagina { get; set; }
        public int TotalPaginas => 
            (int)Math.Ceiling((double)TotalElementos / ElementosPorPagina);
    }
}
