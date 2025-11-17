using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class Libro : Contenido
    {
        public string Autor { get; set; } 
        public Libro(string titulo, string descripcion, string autor, string estado) : base(titulo, descripcion, "Libro", estado) { 
            Autor = autor; 
        }
    }
}