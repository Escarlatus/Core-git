using Asilo.Core.Entities.Financiero;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Implementations;

public class InMemoryMovimientoRepository : IMovimientoRepository
{
    private readonly List<Movimiento> _data = new();
    private int _seq = 1;

    public void Add(Movimiento entity)
    {
        entity.Id = _seq++;
        _data.Add(entity);
    }

    public void Delete(int id) => _data.RemoveAll(x => x.Id == id);
    public IEnumerable<Movimiento> GetAll() => _data;
    public Movimiento? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    public IEnumerable<Movimiento> GetByCuenta(int cuentaId) => _data.Where(x => x.CuentaId == cuentaId);

    public void Update(Movimiento entity)
    {
        var idx = _data.FindIndex(x => x.Id == entity.Id);
        if (idx >= 0)
            _data[idx] = entity;
    }
}
