using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ECommerceWebAppFrontend;
using ECommerceWebAppFrontend.Services; 

namespace ECommerceWebAppFrontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            //en la url alli hay que sustituir por la variable de entorno
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5172") });
            // Agregar el HttpClient para la API de productos
            builder.Services.AddScoped<ProductoService>();
            builder.Services.AddScoped<ClienteService>();
            builder.Services.AddScoped<ProveedorService>();
            builder.Services.AddScoped<PedidoService>();
            builder.Services.AddScoped<ReporteService>();
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<PedidoService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<CategoriaService>();
            builder.Services.AddScoped<MovimientoInventarioService>();
            


            await builder.Build().RunAsync();
        }
    }
}
