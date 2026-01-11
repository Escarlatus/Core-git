using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Implementations;

public class InMemoryResidenteRepository : IResidenteRepository
{
    private readonly List<Residente> _data = new();
    private int _seq = 1;

    public void Add(Residente entity)
    {
        entity.Id = _seq++;
        _data.Add(entity);
    }

    public void Delete(int id) => _data.RemoveAll(x => x.Id == id);
    public IEnumerable<Residente> GetAll() => _data;
    public Residente? GetByDocumento(string documento) => _data.FirstOrDefault(x => x.Documento == documento);
    public Residente? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    public void Update(Residente entity)
    {
        var idx = _data.FindIndex(x => x.Id == entity.Id);
        if (idx >= 0) _data[idx] = entity;
    }
}
