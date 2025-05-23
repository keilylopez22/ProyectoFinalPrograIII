using Microsoft.EntityFrameworkCore;
using ApiECommerce.IServices;
using ApiECommerce.Servicio;
using ApiECommerce.Data; // Asegúrate de tener esta línea

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
/*.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    })*/ // Si vas a crear una API con controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
//builder.Services.AddHostedService<PedidoConsumerService>();

//builder.Services.AddHostedService<KafkaPedidoConsumidor>();
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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // Si vas a crear una API con controladores

app.Run();

// Definición de la clase WeatherForecast (si la necesitas para pruebas)
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}