using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class ListaDeDeseo
    {
        public int IdContenido { get; set; }

        public Serie? Serie {get; set;} = null;

        public Pelicula? Pelicula{get; set;} = null;

        public Libro? Libro{get; set;} = null;
        public Estado Estados { get; set; }

        public enum Estado
        {
            visto,
            porVer, 
            viendo
        }

        public ListaDeDeseo( int idContenido, Estado estado)
        {
            this.IdContenido = idContenido;
            this.Estados = estado;
        }
    }
}