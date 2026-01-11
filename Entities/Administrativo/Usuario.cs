namespace Asilo.Core.Entities.Administrativo;

public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // almacenar hash
    public int RolId { get; set; }
}