using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Enums
{
    public enum EstadoIngreso
    {
        Activo = 1,         // El paciente está en el asilo
        AltaMedica = 2,     // Se recuperó o se retiró (Esta es la que te falta)
        Fallecido = 3,      // Opional
        Traslado = 4        // Opcional
    }
}
