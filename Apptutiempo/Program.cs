
using apptutiempo.Models;
using autonoma3Tiempo.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<registroTemDbContext>(options =>
    options.UseMySQL(connectionString));


var app = builder.Build();
try
{
    string url = "https://api.tutiempo.net/json/?lan=es&apid=zxEq44aqz44t3k9&lid=56608";  // Ruta al archivo JSON
    string json = Http(url);

    Json clave = JsonSerializer.Deserialize<Json>(json);
    Console.WriteLine("Información a Requerida");
    Console.WriteLine($"Pais: {clave.locality.country}");
    Console.WriteLine($"Ciudad: {clave.locality.name}");
    Console.WriteLine($"Fecha: {clave.day1.date}");
    Console.WriteLine($"Temperatura-Max: {clave.day1.temperature_max}");
    Console.WriteLine($"Temperatura-Min: {clave.day1.temperature_min}");

    using var scope = app.Services.CreateScope();
    var dbtemp = scope.ServiceProvider.GetRequiredService<registroTemDbContext>();

    // Consulta utilizando LINQ
    var nuevoitem = new registroTem
    {
        pais = clave.locality.country,  // Puedes ajustar esto según tu modelo
        ciudad = clave.locality.name,
        fecha = clave.day1.date,
        temperatura_maxima = clave.day1.temperature_max,
        temperatura_minima = clave.day1.temperature_min,
       // Otros campos de tu entidad...
    };

    dbtemp.registroTem.Add(nuevoitem);
    dbtemp.SaveChanges();
    Console.WriteLine("Registro Creado Correctamente");

    var data = dbtemp.registroTem
         .ToList();

    foreach (var elemento in data)
    {
        Console.WriteLine($"Registro: {elemento.id_registroTem}");
        Console.WriteLine($"Pais: {elemento.pais}");
        Console.WriteLine($"Ciudad: {elemento.ciudad}");
        Console.WriteLine($"Temperatura Maxima: {elemento.temperatura_maxima}");
        Console.WriteLine($"Temperatura Minima: {elemento.temperatura_minima}");
       
    }

    Console.WriteLine("Alumna Lady Merchan");


}
catch (Exception ex)
{
    Console.WriteLine($"Error al realizar la consulta: {ex.Message}");
}
// Configure the HTTP request pipeline.

static string Http(string url)
{
    WebRequest oRequest = WebRequest.Create(url);
    WebResponse oResponse = oRequest.GetResponse();
    using (StreamReader temp = new StreamReader(oResponse.GetResponseStream()))
    {
        return temp.ReadToEnd().Trim();
    }
}
app.Run();