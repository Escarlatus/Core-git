using Asilo.Core.Entities.Administrativo;
using Asilo.Core.Interfaces;
using Asilo.Core.Rules;
using System.Security.Cryptography;
using System.Text;

namespace Asilo.Core.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _userRepo;
    private readonly AuditService _audit;

    public UsuarioService(IUsuarioRepository userRepo, AuditService audit)
    {
        _userRepo = userRepo;
        _audit = audit;
    }

    public Usuario CrearUsuario(string username, string password, int rolId, string realizadoPor)
    {
        if (!PasswordRules.IsValid(password))
            throw new ArgumentException("Password no cumple reglas de complejidad");

        var existente = _userRepo.GetByUsername(username);
        if (existente != null) throw new InvalidOperationException("Usuario ya existe");

        var user = new Usuario
        {
            Username = username,
            PasswordHash = HashPassword(password),
            RolId = rolId
        };
        _userRepo.Add(user);
        _audit.Log(realizadoPor, "CREAR_USUARIO", $"Usuario {username} creado");
        return user;
    }

    public Usuario? Authenticate(string username, string password)
    {
        var u = _userRepo.GetByUsername(username);
        if (u == null) return null;
        var hash = HashPassword(password);
        if (u.PasswordHash != hash) return null;
        _audit.Log(username, "LOGIN", "Autenticación exitosa");
        return u;
    }

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}