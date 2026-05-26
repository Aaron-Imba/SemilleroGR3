using System;
using System.Collections.Generic;
using System.Text;

namespace SemilleroGR3.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string Email { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }

        // Propiedad auxiliar para manejar el JWT en la sesión activa
        public string Token { get; set; }
    }
}
