using System;
using System.Collections.Generic;
using System.Text;

namespace SemilleroGR3.Models
{
    public class TareaCasa
    {
        public int Id { get; set; } // Id del TareaCasa_Seguimiento
        public int ActividadId { get; set; }
        public string TituloActividad { get; set; }      // Viene de Actividad
        public string DescripcionActividad { get; set; } // Viene de Actividad
        public int AlumnoId { get; set; }

        // Campos interactivos para la familia
        public bool Realizada { get; set; }
        public string ComentarioBreve { get; set; }
        public DateTime? FechaReporte { get; set; }
    }
}
