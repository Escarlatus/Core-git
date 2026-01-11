using Asilo.Core.Entities.Financiero;

namespace Asilo.Core.Interfaces;

public interface IMovimientoRepository : IRepository<Movimiento>
{
    IEnumerable<Movimiento> GetByCuenta(int cuentaId);
}