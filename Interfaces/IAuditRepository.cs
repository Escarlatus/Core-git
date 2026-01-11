using Asilo.Core.Entities.Administrativo;

namespace Asilo.Core.Interfaces;

public interface IAuditRepository
{
    void Log(AuditRecord record);
    IEnumerable<AuditRecord> GetAll();
}