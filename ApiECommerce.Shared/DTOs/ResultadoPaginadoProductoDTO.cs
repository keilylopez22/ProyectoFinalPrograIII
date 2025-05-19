using ApiECommerce.DTOs; // Cambia el namespace según corresponda

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
