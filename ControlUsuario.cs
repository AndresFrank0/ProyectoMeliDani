/*using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestProject.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => { options.AddPolicy(name: "AllowAll", policy => { policy.AllowAnyOrigin() .AllowAnyHeader() .AllowAnyMethod(); }); });

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var archivoJson = "usuarios.json";

app.MapPost("/controlUsuario", (List<Persona> nuevasPersonas) => { 
    var personasExistentes = CargarPersonas(); 
    foreach (var nuevaPersona in nuevasPersonas) { 
        if (personasExistentes.Any(p => p.Email == nuevaPersona.Email || p.Usuario == nuevaPersona.Usuario)) { 
            return Results.BadRequest(new { message = "El correo o el usuario ya existe" }); } 
            else { 
                personasExistentes.Add(nuevaPersona); 
                } 
        } 
        var json = JsonConvert.SerializeObject(personasExistentes, Formatting.Indented); 
        File.WriteAllText(archivoJson, json); return Results.Ok(new { message = "Personas guardadas con éxito" });
    }) 
.WithName("GuardarPersonas")
.WithOpenApi();

app.MapGet("/api/controlUsuario/{usuario}", (string usuario) =>
{
    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == usuario);

    if (persona == null)
    {
        return Results.NotFound(new { message = "Usuario no encontrado" });
    }

    var resultado = new
    {
        persona.Nombre,
        persona.Usuario,
        persona.Edad,
        persona.Email
    };

    return Results.Ok(resultado);
})
.WithName("ObtenerPerfil")
.WithOpenApi();




app.MapPost("/login", (LoginRequest loginRequest) =>
{
    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == loginRequest.Usuario && p.Contraseña == loginRequest.Contraseña);

    if (persona == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { message = "Inicio de sesión exitoso" });
})
.WithName("IniciarSesion")
.WithOpenApi();

app.MapPost("/contenido", async (HttpContext context) =>
{
    using var reader = new StreamReader(context.Request.Body);
    var body = await reader.ReadToEndAsync();
    var jObject = JObject.Parse(body);

    var usuario = jObject["usuario"]?.ToString();
    var contenido = jObject["contenido"]?.ToObject<Contenido>(new JsonSerializer
    {
        TypeNameHandling = TypeNameHandling.Auto
    });

    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == usuario);

    if (persona == null)
    {
        return Results.NotFound(new { message = "Usuario no encontrado" });
    }

    // Verificar si el contenido ya existe en la lista de deseos
    bool contenidoYaExiste = persona.ListaDeseo.Any(c => c.Titulo == contenido.Titulo);
    if (contenidoYaExiste)
    {
        return Results.BadRequest(new { message = "El contenido ya está en la lista de deseos" });
    }

    persona.ListaDeseo.Add(contenido);

    var json = JsonConvert.SerializeObject(personas, Formatting.Indented, new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto
    });
    await File.WriteAllTextAsync(archivoJson, json);

    return Results.Ok(new { message = "Contenido agregado a la lista de deseos con éxito" });
})
.WithName("GuardarContenidoEnListaDeseo")
.WithOpenApi();


app.MapGet("/contenido/{usuario}", (string usuario) =>
{
    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == usuario);

    if (persona == null)
    {
        return Results.NotFound(new { message = "Usuario no encontrado" });
    }

    return Results.Ok(persona.ListaDeseo);
})
.WithName("ObtenerListaDeDeseos")
.WithOpenApi();




app.Run();

List<Persona> CargarPersonas()
{
    if (!File.Exists(archivoJson))
    {
        return new List<Persona>();
    }

    var json = File.ReadAllText(archivoJson);
    return JsonConvert.DeserializeObject<List<Persona>>(json, new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto
    });
}
*/