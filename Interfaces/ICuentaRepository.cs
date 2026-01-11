using Asilo.Core.Entities.Financiero;

namespace Asilo.Core.Interfaces;

public interface ICuentaRepository : IRepository<Cuenta>
{
    Cuenta? GetByResidente(int residenteId);
}