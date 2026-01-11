using Asilo.Core.Entities.Financiero;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Implementations;

public class InMemoryCuentaRepository : ICuentaRepository
{
    private readonly List<Cuenta> _data = new();
    private int _seq = 1;

    public void Add(Cuenta entity)
    {
        entity.Id = _seq++;
        _data.Add(entity);
    }

    public void Delete(int id) => _data.RemoveAll(x => x.Id == id);
    public IEnumerable<Cuenta> GetAll() => _data;
    public Cuenta? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    public Cuenta? GetByResidente(int residenteId) => _data.FirstOrDefault(x => x.ResidenteId == residenteId);

    public void Update(Cuenta entity)
    {
        var idx = _data.FindIndex(x => x.Id == entity.Id);
        if (idx >= 0)
            _data[idx] = entity;
    }
}
