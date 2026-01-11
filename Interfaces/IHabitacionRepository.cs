using Asilo.Core.Entities.Medico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asilo.Core.Interfaces
{
    public interface IHabitacionRepository : IRepository<Habitacion>
    {
        // Método para obtener solo las habitaciones que no están llenas
        IEnumerable<Habitacion> GetDisponibles();
    }
}
