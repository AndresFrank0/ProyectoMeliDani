using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class Persona
    {
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public int Edad { get; set; }

        public List<Contenido> ListaDeseo { get; set; } = new List<Contenido>();

        public Persona(string nombre, string usuario, string contraseña, string email, int edad)
        {
            this.Nombre = nombre;
            this.Usuario = usuario;
            this.Contraseña = contraseña;
            this.Email = email;
            this.Edad = edad;
            
        }
    }
}