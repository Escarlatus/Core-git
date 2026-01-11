namespace Asilo.Core.Entities.Administrativo;

public class AuditRecord
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Usuario { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Detalles { get; set; } = string.Empty;
}
