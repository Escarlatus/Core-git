using Asilo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Entities.Medico
{
    public class Ingreso
    {
        public int Id { get; set; }
        public int ResidenteId { get; set; } // El paciente
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaAlta { get; set; } // Puede ser nulo si sigue hospitalizado
        public string DiagnosticoEntrada { get; set; } = string.Empty;
        public int? HabitacionId { get; set; } // Opcional
        public EstadoIngreso Estado { get; set; } // Activo o Alta
    }
}
