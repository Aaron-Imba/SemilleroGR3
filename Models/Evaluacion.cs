using System;
using System.Collections.Generic;
using System.Text;

namespace SemilleroGR3.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public int RubricaId { get; set; }
        public int DocenteId { get; set; }
        public int AlumnoId { get; set; }
        public int? UnidadId { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public string NotaGeneral { get; set; }

        // Lista dinámica con los criterios evaluados
        public List<DetalleEvaluacion> Detalles { get; set; } = new List<DetalleEvaluacion>();
    }
    public class DetalleEvaluacion
    {
        public int Id { get; set; }
        public int CriterioId { get; set; }
        public string CriterioNombre { get; set; } // Ej: "Clasificación" (Resuelto en Backend)
        public int NivelId { get; set; }
        public string NivelNombre { get; set; }    // Ej: "Logrado" (Resuelto en Backend)
        public string ObservacionEspecifica { get; set; }
    }
}
