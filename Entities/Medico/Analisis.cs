using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Entities.Medico
{
    public class Analisis
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        // Podrías agregar 'TipoMuestra' si quieres ser más detallista
    }
}
