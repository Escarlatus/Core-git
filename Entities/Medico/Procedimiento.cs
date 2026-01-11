using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Entities.Medico
{
    public class Procedimiento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; } // Importante para la Caja
        public bool Activo { get; set; } = true;
    }
}
