using Asilo.Core.Entities.Administrativo;

namespace Asilo.Core.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Usuario? GetByUsername(string username);
}