using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class Serie : Contenido
    {  
        public Serie(string titulo, string descripcion, string estado) : base(titulo, descripcion, "Serie", estado) 
        { 
        }
    }
}