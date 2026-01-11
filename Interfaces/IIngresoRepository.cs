using Asilo.Core.Entities.Medico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Interfaces
{
    public interface IIngresoRepository : IRepository<Ingreso>
    {
        Ingreso? GetIngresoActivo(int residenteId); // Ver si el paciente está ingresado ahora
    }
}
