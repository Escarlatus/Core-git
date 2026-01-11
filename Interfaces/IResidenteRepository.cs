using Asilo.Core.Entities.Medico;

namespace Asilo.Core.Interfaces;

public interface IResidenteRepository : IRepository<Residente>
{
    Residente? GetByDocumento(string documento);
}