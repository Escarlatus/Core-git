using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Implementations;

public class InMemoryAuditRepository : IAuditRepository
{
    private readonly List<AuditRecord> _data = new();
    private int _seq = 1;

    public void Log(AuditRecord record)
    {
        record.Id = _seq++;
        _data.Add(record);
    }

    public IEnumerable<AuditRecord> GetAll() => _data;
}
