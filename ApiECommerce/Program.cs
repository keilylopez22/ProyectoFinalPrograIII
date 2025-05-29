using Microsoft.EntityFrameworkCore;
using ApiECommerce.IServices;
using ApiECommerce.Servicio;
using ApiECommerce.Data;
using System.Reflection;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
// Agregar servicios al contenedor.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Incluir los comentarios XML en Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurar la conexión a la base de datos MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IClienteService, ClienteServicio>();
builder.Services.AddScoped<IReporteServicio, ReporteServicio>();
builder.Services.AddScoped<IComprasServicio, CompraServicio>();
builder.Services.AddScoped<IPedidosServicio, PedidoServicio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IKafkaProductorServicio, KafkaProductorServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IMovimientosInventarioServicio, MovimientosInventarioServicio>();
builder.Services.AddScoped<IProveedoresServicio, ProveedorServicio>();
builder.Services.AddHostedService<PedidoConsumerService>();


builder.Services.AddHostedService<KafkaPedidoConsumidor>();
//builder.Services.AddScoped<LoginServicio>();



//habilitar cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5056") // ← URL del frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("PermitirFrontend");

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configurar la autenticación y autorización
/*public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddEnvironmentVariables(); // Esto ya viene en Host.CreateDefaultBuilder
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });*/


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
