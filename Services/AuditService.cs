using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Services;

public class AuditService
{
    private readonly IAuditRepository _repo;

    public AuditService(IAuditRepository repo)
    {
        _repo = repo;
    }

    public void Log(string usuario, string accion, string detalles)
    {
        var record = new AuditRecord
        {
            Usuario = usuario,
            Accion = accion,
            Detalles = detalles,
            Timestamp = DateTime.UtcNow
        };
        _repo.Log(record);
    }
}