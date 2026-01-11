namespace Asilo.Core.Entities.Medico;

public class Residente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string EstadoSalud { get; set; } = string.Empty;
    public int? HabitacionId { get; set; }
}