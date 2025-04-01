using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // 

using System;
using ApiECommerce;
using ApiECommerce.Data; // Asegúrate de que este namespace es el correcto
var builder = WebApplication.CreateBuilder(args);


// Configuración de la conexión a la base de datos MySQL
builder.Services.AddDbContext<ECommersContext>(options =>
    options.UseMySql(
        //"server=sql5.freesqldatabase.com;port=3306;database=sql5769382;user=sql5769382;password=AdGnpPleA8;",
        "server=localhost;port=3306;database=programacionIII;user=proyecto;password=panfila;",
        new MySqlServerVersion(new Version(8, 0, 30))
    ));

// Añadir servicios de controladores

builder.Services.AddControllers();

//--------------------------------------------


//---------------------------------------


//  AGREGAR ESTO para Swagger 
builder.Services.AddEndpointsApiExplorer();  // Habilita soporte para endpoints (requerido por Swagger)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiECommerce", Version = 
    "v1" });
});
//  termino swagger
//------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //chat
    app.UseDeveloperExceptionPage();
    // ✅ Habilitar Swagger y su interfaz
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ECommerce v1"));

}

app.UseHttpsRedirection();




app.UseRouting();          // Necesario para enrutar las solicitudes a tus controladores
app.UseAuthorization();     // Si usas autenticación/authorization en tu proyecto
app.MapControllers();
 



var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
//ejemplo que dio el proyecto al crear la webApi
/*app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");*/

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
