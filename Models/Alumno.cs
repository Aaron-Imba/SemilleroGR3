using System;
using System.Collections.Generic;
using System.Text;

namespace SemilleroGR3.Models
{
    public class Alumno
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Activo { get; set; }
    }
}
