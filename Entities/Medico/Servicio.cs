namespace Asilo.Core.Entities.Medico;

public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // e.g., Terapia física
    public decimal Precio { get; set; }
}