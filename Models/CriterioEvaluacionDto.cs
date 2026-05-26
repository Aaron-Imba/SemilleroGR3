using System;
using System.Collections.Generic;
using System.Text;

namespace SemilleroGR3.Models
{
    public class CriterioEvaluacionDto
    {
        public int CriterioId { get; set; }
        public string NombreCriterio { get; set; } // Ej: "Clasificación", "Seriación"
        public string DescripcionCriterio { get; set; }
        public string CodigoLogro { get; set; } // Ej: "1", "EP", "L"
        public string Observacion { get; set; } // Nota del docente
    }
}
