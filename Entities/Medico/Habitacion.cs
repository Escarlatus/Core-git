namespace Asilo.Core.Entities.Medico;

public class Habitacion
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int Capacidad { get; set; } = 1;
    public List<int> ResidenteIds { get; set; } = new();
}