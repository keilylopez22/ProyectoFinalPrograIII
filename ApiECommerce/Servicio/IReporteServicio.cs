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

namespace ProyectoFinal_PrograIII.Servicio
{
    public interface IReporteServicio
    {
        Task<byte[]>GenerarReporteCompras();

         Task<byte[]>GenerarReportePedidos();

         
    }
    public class ReporteServicio:IReporteServicio
    {
         public async Task<byte[]>GenerarReporteCompras()
         {
            //aqui estoy crreando el archivo excel 
            using(var workbook= new XSSFWorkbook())
            {
                var hojaCompras = workbook.CreateSheet("compras");
                using(var stream= new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }

         }

         public async Task<byte[]>GenerarReportePedidos()
         {
            //aqui estoy crreando el archivo excel 
            using(var workbook= new XSSFWorkbook())
            {
                var hojaPedidos = workbook.CreateSheet("pedidos");
                using(var stream= new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }

         }


    }
}