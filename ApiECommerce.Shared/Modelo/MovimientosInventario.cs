using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace ApiECommerce.Modelo
{
    public class MovimientosInventario
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [EnumDataType(typeof(TipoMovimiento))]
        public TipoMovimiento TipoMovimiento { get; set; }

        [Required]
        public int Cantidad { get; set; }

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [Required]
        [EnumDataType(typeof(TipoDocumento))]
        public TipoDocumento TipoDocumento { get; set; }

        [Required]
        public int IdDocumento { get; set; }

        public string? Notas { get; set; }

        // Relación con producto
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        public decimal PrecioUnitario { get; set; }
    }
    // <summary>
    // Enumeración para el tipo de movimiento
    // </summary>
    // <remarks>
    // entrada: Movimiento de entrada de inventario
    // salida: Movimiento de salida de inventario
    // </remarks>
    
    public enum TipoMovimiento
    {
        entrada,
        salida
    }

    public enum TipoDocumento
    {
        compra ,
        pedido , 
        ajuste,
        devolucion
    }

}