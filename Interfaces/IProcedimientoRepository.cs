using Asilo.Core.Entities.Medico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Interfaces
{
    public interface IProcedimientoRepository : IRepository<Procedimiento>
    {
        // Método extra útil: Buscar por nombre para autocompletar en Caja
        IEnumerable<Procedimiento> BuscarPorNombre(string nombre);
    }
}
