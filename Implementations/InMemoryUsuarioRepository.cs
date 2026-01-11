using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Implementations;

public class InMemoryUsuarioRepository : IUsuarioRepository
{
    private readonly List<Usuario> _data = new();
    private int _seq = 1;

    public void Add(Usuario entity)
    {
        entity.Id = _seq++;
        _data.Add(entity);
    }

    public void Delete(int id) => _data.RemoveAll(x => x.Id == id);
    public IEnumerable<Usuario> GetAll() => _data;
    public Usuario? GetById(int id) => _data.FirstOrDefault(x => x.Id == id);
    public Usuario? GetByUsername(string username) => _data.FirstOrDefault(x => x.Username == username);

    public void Update(Usuario entity)
    {
        var idx = _data.FindIndex(x => x.Id == entity.Id);
        if (idx >= 0)
            _data[idx] = entity;
    }
}
