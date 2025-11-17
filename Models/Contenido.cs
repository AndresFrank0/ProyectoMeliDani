using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
public class Contenido
{
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? FotoPortada { get; set; }
    public string? Tipo { get; set; }
    public string? Estado { get; set; }

    // Constructor sin parámetros
    public Contenido() { }

    public Contenido(string titulo, string descripcion, string tipo, string estado, string? fotoPortada = null)
    {
        Titulo = titulo;
        Descripcion = descripcion;
        Tipo = tipo;
        Estado = estado;
        FotoPortada = fotoPortada;
    }
}
}