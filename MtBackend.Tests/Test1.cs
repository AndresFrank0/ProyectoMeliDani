using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

// Asegúrate de que los namespaces sean correctos
using TestProject.Controllers; // El namespace de tu controlador
using TestProject.Models;     // El namespace de tus modelos (Contenido, Persona)

[DoNotParallelize]
[TestClass]
public class ControladorContenidoTests
{
    // El nombre del archivo DEBE COINCIDIR con el de tu controlador
    private const string _archivoJsonPrueba = "usuarios.json";
    
    // Nuestro controlador que probaremos
    // <-- CORRECCIÓN: Agregamos '?' para solucionar la advertencia CS8618
    private ControladorContenido? _controlador;

    // --- CONFIGURACIÓN DE PRUEBA ---

    [TestInitialize] // Esto se ejecuta ANTES de CADA prueba
    public void Setup()
    {
        // 1. Limpiamos cualquier archivo viejo
        if (File.Exists(_archivoJsonPrueba))
        {
            File.Delete(_archivoJsonPrueba);
        }

        // 2. Creamos datos de prueba
        // <-- CORRECCIÓN: Llenamos todos los campos de 'Contenido' para evitar warnings.
        var contenidoExistente = new Contenido 
        { 
            Titulo = "Pelicula Antigua", 
            Estado = "visto",
            Descripcion = "Descripción de prueba", // Asume que estos campos existen
            FotoPortada = "url-foto.jpg",
            Tipo = "Pelicula"
        };

        // <-- CORRECCIÓN: Error CS7036. 
        // Tu clase 'Persona' necesita 5 argumentos en su constructor.
        // ¡DEBES REEMPLAZAR ESTOS VALORES CON LOS CORRECTOS PARA TU CONSTRUCTOR!
        var personaDePrueba = new Persona(
            "NombreDePrueba",     // 1er argumento (string nombre?)
            "testuser",           // 3er argumento (string usuario?)
            "pass123",            // 4to argumento (string contraseña?)
            "email@prueba.com",   // 2do argumento (string? email)            
            25                    // 5to argumento (int edad?)
        );
        
        // Asignamos la lista de deseos a la persona que acabamos de crear
        personaDePrueba.ListaDeseo = new List<Contenido> { contenidoExistente };

        // Creamos la lista de personas para guardar en JSON
        var personas = new List<Persona> { personaDePrueba };


        // 3. Escribimos esos datos al archivo .json que el controlador leerá
        var json = JsonConvert.SerializeObject(personas, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(_archivoJsonPrueba, json);

        // 4. Creamos una nueva instancia del controlador para la prueba
        _controlador = new ControladorContenido();
    }

    [TestCleanup] // Esto se ejecuta DESPUÉS de CADA prueba
    public void Cleanup()
    {
        // Borramos el archivo .json para que las pruebas no interfieran entre sí
        if (File.Exists(_archivoJsonPrueba))
        {
            File.Delete(_archivoJsonPrueba);
        }
    }

    // --- PRUEBAS PARA GuardarContenido ---

    [TestMethod]
    public async Task GuardarContenido_Exitoso_DebeDevolverOkYAgregarContenido()
    {
        // 1. ARRANGE (Preparar)
        var usuario = "testuser"; // Usuario que SÍ existe (creado en Setup)
        // <-- CORRECCIÓN: Llenamos todos los campos para evitar warnings
        var nuevoContenido = new Contenido
        {
            Titulo = "Pelicula Nueva",
            Estado = "porver", // Estado válido
            Descripcion = "Desc...",
            FotoPortada = "url.jpg",
            Tipo = "Pelicula"
        };

        // 2. ACT (Actuar)
        // Ejecutamos la función asíncrona y esperamos el resultado
        // Usamos '_controlador!' para decirle al compilador que NO es nulo aquí
        var resultado = await _controlador!.GuardarContenido(usuario, nuevoContenido);

        // 3. ASSERT (Verificar)
        Assert.IsInstanceOfType(resultado, typeof(OkObjectResult));

        // Verificación extra:
        var personas = CargarPersonasDirectamente(); 
        var persona = personas.FirstOrDefault(p => p.Usuario == usuario);
        Assert.IsNotNull(persona);
        Assert.AreEqual(2, persona.ListaDeseo.Count); 
        Assert.IsTrue(persona.ListaDeseo.Any(c => c.Titulo == "Pelicula Nueva"));
    }

    [TestMethod]
    public async Task GuardarContenido_UsuarioNoExiste_DebeDevolverNotFound()
    {
        // 1. ARRANGE
        var usuario = "usuarioFALSO"; 
        // <-- CORRECCIÓN: Llenamos todos los campos
        var nuevoContenido = new Contenido 
        { 
            Titulo = "Pelicula", 
            Estado = "visto",
            Descripcion = "Desc...",
            FotoPortada = "url.jpg",
            Tipo = "Pelicula"
        };

        // 2. ACT
        var resultado = await _controlador!.GuardarContenido(usuario, nuevoContenido);

        // 3. ASSERT
        Assert.IsInstanceOfType(resultado, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task GuardarContenido_ContenidoYaExiste_DebeDevolverBadRequest()
    {
        // 1. ARRANGE
        var usuario = "testuser";
        // <-- CORRECCIÓN: Llenamos todos los campos
        var contenidoDuplicado = new Contenido 
        { 
            Titulo = "Pelicula Antigua", // Título duplicado
            Estado = "viendo",
            Descripcion = "Desc...",
            FotoPortada = "url.jpg",
            Tipo = "Pelicula"
        };

        // 2. ACT
        var resultado = await _controlador!.GuardarContenido(usuario, contenidoDuplicado);

        // 3. ASSERT
        Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GuardarContenido_EstadoInvalido_DebeDevolverBadRequest()
    {
        // 1. ARRANGE
        var usuario = "testuser";
        // <-- CORRECCIÓN: Llenamos todos los campos
        var contenidoInvalido = new Contenido 
        { 
            Titulo = "Pelicula Rara", 
            Estado = "QUIZAS", // Estado inválido
            Descripcion = "Desc...",
            FotoPortada = "url.jpg",
            Tipo = "Pelicula"
        };

        // 2. ACT
        var resultado = await _controlador!.GuardarContenido(usuario, contenidoInvalido);

        // 3. ASSERT
        Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task GuardarContenido_ContenidoNulo_DebeDevolverBadRequest()
    {
        // 1. ARRANGE
        var usuario = "testuser";
        Contenido? contenidoNulo = null; // <-- CORRECCIÓN: Usamos '?' para tipos nulables

        // 2. ACT
        var resultado = await _controlador!.GuardarContenido(usuario, contenidoNulo!);

        // 3. ASSERT
        Assert.IsInstanceOfType(resultado, typeof(BadRequestObjectResult));
    }


    // --- Método Helper ---
    // (Copiado de tu controlador para verificar el estado final del archivo)
    private List<Persona> CargarPersonasDirectamente()
    {
        if (!File.Exists(_archivoJsonPrueba))
        {
            return new List<Persona>();
        }
        var json = File.ReadAllText(_archivoJsonPrueba);
        return JsonConvert.DeserializeObject<List<Persona>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? new List<Persona>();
    }
}