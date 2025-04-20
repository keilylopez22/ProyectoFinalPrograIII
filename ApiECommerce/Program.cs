using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using ProyectoFinal_PrograIII.Servicio;
using ProyectoFinal_PrograIII.Data; // Asegúrate de tener esta línea

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers(); // Si vas a crear una API con controladores
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // Si vas a crear una API con controladores

app.Run();

// Definición de la clase WeatherForecast (si la necesitas para pruebas)
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}