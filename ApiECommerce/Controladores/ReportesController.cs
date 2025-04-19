using Microsoft.AspNetCore.Mvc; 
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoFinal_PrograIII.Servicio;
namespace ProyectoFinal_PrograIII.Controladores


//namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]  // Esto hará que las URLs sean /api/proveedores
    [ApiController]

    public class ReportesController: ControllerBase
    {
        private readonly IReporteServicio _reporteServicio;
        
        public ReportesController(IReporteServicio reporteServicio)
        {
            //inyectar instamcia del servicio para que se pueda utilizar dentro del controlador  
            _reporteServicio = reporteServicio;
        }

         [HttpGet("compras")]
        public async Task<IActionResult> GetReporteCompras()
        {
            var archivo = await _reporteServicio.GenerarReporteCompras();
            //aqui se esta retornando el reporte en formato excel
            return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteCompras.xlsx");

        }
        [HttpGet("pedidos")]
        public async Task<IActionResult> GetReportePedidos()
        {
            var archivo = await _reporteServicio.GenerarReportePedidos();
            //aqui se esta retornando el reporte en formato excel
            return File(archivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePedidos.xlsx");

        }

    }
}
