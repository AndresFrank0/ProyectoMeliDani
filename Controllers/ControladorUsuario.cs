using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestProject.Models;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("api/controlUsuario")]
    public class ControladorUsuario : ControllerBase
    {
        private readonly string archivoJson = "usuarios.json";

        [HttpPost]
        public IActionResult GuardarPersonas([FromBody] List<Persona> nuevasPersonas)
        {
            var personasExistentes = CargarPersonas();
            foreach (var nuevaPersona in nuevasPersonas)
            {
                if (personasExistentes.Any(p => p.Email == nuevaPersona.Email || p.Usuario == nuevaPersona.Usuario))
                {
                    return BadRequest(new { message = "El correo o el usuario ya existe" });
                }
                else
                {
                    personasExistentes.Add(nuevaPersona);
                }
            }
            var json = JsonConvert.SerializeObject(personasExistentes, Formatting.Indented);
            System.IO.File.WriteAllText(archivoJson, json);
            return Ok(new { message = "Personas guardadas con éxito" });
        }

        [HttpGet("{usuario}")]
        public IActionResult ObtenerPerfil(string usuario)
        {
            var personas = CargarPersonas();
            var persona = personas.FirstOrDefault(p => p.Usuario == usuario);

            if (persona == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var resultado = new
            {
                persona.Nombre,
                persona.Usuario,
                persona.Edad,
                persona.Email
            };

            return Ok(resultado);
        }

        [HttpPost("login")]
        public IActionResult IniciarSesion([FromBody] LoginRequest loginRequest)
        {
            var personas = CargarPersonas();
            var persona = personas.FirstOrDefault(p => p.Usuario == loginRequest.Usuario && p.Contraseña == loginRequest.Contraseña);

            if (persona == null)
            {
                return Unauthorized();
            }

            return Ok(new { message = "Inicio de sesión exitoso" });
        }

        [HttpGet("obtenerContrasena/{usuario}")]
public IActionResult ObtenerContrasena(string usuario)
{
    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == usuario);

    if (persona == null)
    {
        return NotFound(new { message = "Usuario no encontrado" });
    }

    var resultado = new
    {
        Contrasena = persona.Contraseña // Supongamos que tienes un campo 'Contrasena' en tu modelo de datos
    };

    return Ok(resultado);
}


[HttpPut("modificarPerfil")]
public IActionResult ModificarPerfil([FromBody] Persona perfilModificado)
{
    Console.WriteLine("Solicitud recibida para modificar perfil.");
    Console.WriteLine($"Datos recibidos: {JsonConvert.SerializeObject(perfilModificado)}");

    var personas = CargarPersonas();
    var persona = personas.FirstOrDefault(p => p.Usuario == perfilModificado.Usuario);

    if (persona == null)
    {
        return NotFound(new { message = "Usuario no encontrado" });
    }

    // Validar datos del perfil modificado
    if (string.IsNullOrEmpty(perfilModificado.Nombre) || string.IsNullOrEmpty(perfilModificado.Email) || string.IsNullOrEmpty(perfilModificado.Usuario))
    {
        return BadRequest(new { message = "Datos no válidos. Asegúrate de que todos los campos requeridos estén completos." });
    }

    // Validar que el nuevo nombre de usuario no esté en uso por otro usuario
    if (personas.Any(p => p.Usuario == perfilModificado.Usuario && p.Email != persona.Email))
    {
        return BadRequest(new { message = "El nombre de usuario ya existe" });
    }

    // Copiar la contraseña existente al perfil modificado
    perfilModificado.Contraseña = persona.Contraseña;

    // Modificar datos del perfil
    persona.Nombre = perfilModificado.Nombre;
    persona.Usuario = perfilModificado.Usuario; // Cambiar nombre de usuario
    persona.Edad = perfilModificado.Edad;
    persona.Email = perfilModificado.Email;
    persona.Contraseña = perfilModificado.Contraseña; // Asegurar que la contraseña se mantiene igual

    var json = JsonConvert.SerializeObject(personas, Formatting.Indented);
    System.IO.File.WriteAllTextAsync(archivoJson, json);

    Console.WriteLine("Perfil modificado con éxito.");

    return Ok(new { message = "Perfil modificado con éxito" });
}


[HttpDelete("eliminarCuenta/{usuario}")]
public IActionResult EliminarCuenta(string usuario)
{
    var personas = CargarPersonas();
    var user = personas.FirstOrDefault(u => u.Usuario == usuario);
    if (user == null)
    {
        return NotFound(new { message = "Usuario no encontrado" });
    }
    
    // Limpiar la lista de deseos del usuario
    user.ListaDeseo.Clear();

    // Remover el usuario de la lista de usuarios
    personas.Remove(user);

    // Guardar la lista actualizada de usuarios
    var json = JsonConvert.SerializeObject(personas, Formatting.Indented);
    System.IO.File.WriteAllText(archivoJson, json);

    return Ok(new { message = "Cuenta y todos los datos asociados eliminados exitosamente" });
}






        private List<Persona> CargarPersonas()
        {
            if (!System.IO.File.Exists(archivoJson))
            {
                return new List<Persona>();
            }

            var json = System.IO.File.ReadAllText(archivoJson);
            return JsonConvert.DeserializeObject<List<Persona>>(json);
        }
    }
}

