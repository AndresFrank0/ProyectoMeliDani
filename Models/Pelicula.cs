using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class Pelicula : Contenido
    {
    public Pelicula(string titulo, string descripcion, string estado) : base(titulo, descripcion, "Pelicula", estado) { }
    }
}