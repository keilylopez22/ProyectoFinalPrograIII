using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using ECommerceWebAppFrontend;
using ECommerceWebAppFrontend.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace ECommerceWebAppFrontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Configurar HttpClient base
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5172") });

            // Registrar servicios
            builder.Services.AddScoped<ProductoService>();
            builder.Services.AddScoped<ClienteService>();
            builder.Services.AddScoped<ProveedorService>();
            builder.Services.AddScoped<PedidoService>();
            builder.Services.AddScoped<ReporteService>();
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<CategoriaService>();
            builder.Services.AddScoped<MovimientoInventarioService>();
            builder.Services.AddScoped<LoginService>();

            // Blazored LocalStorage
            builder.Services.AddBlazoredLocalStorage();

            // Autenticación personalizada
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<CustomAuthStateProvider>());

            await builder.Build().RunAsync();
        }
    }
}
