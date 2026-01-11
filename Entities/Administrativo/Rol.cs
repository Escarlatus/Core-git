namespace Asilo.Core.Entities.Administrativo;

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // e.g., ADMIN, CAJERO, MEDICO, CONSULTA
}