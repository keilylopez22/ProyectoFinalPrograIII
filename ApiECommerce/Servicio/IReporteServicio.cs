using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApiECommerce.Servicio
{
    public interface IReporteServicio
    {
        Task<byte[]>GenerarReporteCompras(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProductos = null,
            int? IdProveedor = null);
        
        

         Task<byte[]>GenerarReportePedidos(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProducto = null,
            int? IdCliente = null,
            int? IdProveedor = null);

         
    }
    public class ReporteServicio:IReporteServicio
    {

        private readonly IComprasServicio _comprasServicio;
        private readonly IPedidosServicio _pedidosServicio;


        public ReporteServicio(IComprasServicio comprasServicio, IPedidosServicio pedidosServicio)
        {
            _comprasServicio = comprasServicio;
            _pedidosServicio = pedidosServicio;

        }
         public async Task<byte[]>GenerarReporteCompras(
           DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProductos = null,
            int? IdProveedor = null)
         {
            //consulta que devuelve la lista de compras
            var compras =  await _comprasServicio.ObtenerComprasAsync
            (
            fechaInicio,
            fechaFin,
            IdProductos,
            IdProveedor);
      

            //aqui estoy crreando el archivo excel 
            using(var workbook= new XSSFWorkbook())
            {
                
                var hojaCompras = workbook.CreateSheet("compras");
                generarEncabezados(hojaCompras, new []{"Id", "Total", "Fecha", "Estado", "Id Proveedor","Proveedor"});
                int fila = 1;
                //rrecorre toda la lista de compras y almacena la inf en una variable compra
                foreach(var compra in compras)
                {
                    //creamos la fila
                    var filaDeDatos = hojaCompras.CreateRow(fila++);
                     //creamos  celda
                     filaDeDatos.CreateCell(0).SetCellValue(compra.Id);
                     filaDeDatos.CreateCell(1).SetCellValue(compra.Total);
                     filaDeDatos.CreateCell(2).SetCellValue(compra.Fecha.ToString("dd/MM/yyyy"));
                     filaDeDatos.CreateCell(3).SetCellValue(compra.Estado);
                     filaDeDatos.CreateCell(4).SetCellValue(compra.IdProveedor);
                     filaDeDatos.CreateCell(5).SetCellValue(compra.Proveedor.Nombre);



                }
                using(var stream= new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }

         }

         public async Task<byte[]>GenerarReportePedidos(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            int? IdProducto = null,
            int? IdCliente = null,
            int? IdProveedor = null
            )
         
         {
            
            var pedidos = await _pedidosServicio.ObtenerPedidosAsync(
            fechaInicio,
            fechaFin,
            IdProducto,
            IdCliente,
            IdProveedor);
            //aqui estoy crreando el archivo excel 
            using(var workbook= new XSSFWorkbook())
            {
                var hojaPedidos = workbook.CreateSheet("pedidos");
                generarEncabezados(hojaPedidos, new []{"Id", "Total", "Fecha", "Estado", "Id Cliente", "cliente"});

                int fila =1;
                foreach(var pedido in pedidos)
                {
                    var filaDeDatos = hojaPedidos.CreateRow(fila++);
                    filaDeDatos.CreateCell(0).SetCellValue(pedido.Id);
                    filaDeDatos.CreateCell(1).SetCellValue(pedido.Total);
                    filaDeDatos.CreateCell(2).SetCellValue(pedido.Fecha.ToString("dd/MM/yyyy"));
                    filaDeDatos.CreateCell(3).SetCellValue(pedido.Estado);
                    filaDeDatos.CreateCell(4).SetCellValue(pedido.IdCliente);
                    filaDeDatos.CreateCell(5).SetCellValue(pedido.Cliente.Nombre);

                }

                //MemoryStream para reservar un espacio en memori para guardr el archivo
                using(var stream= new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }

         }

         private void generarEncabezados(ISheet hoja, string[] encabezados)
         {
            //creamos la fila iniciendo con fila 0
            var filaEncabezados = hoja.CreateRow(0);

            //creamos las columnas 
            for(int i = 0; i < encabezados.Length; i++)
            {
                //con el CreateCell creo las celdas y con SetCellValue le doy valor a esas celdas
                filaEncabezados.CreateCell(i).SetCellValue(encabezados[i]);
            }
            
                
            

         }


    }
}