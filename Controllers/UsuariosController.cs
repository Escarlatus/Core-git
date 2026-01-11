using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Services;
using Asilo.Core.Interfaces;
using Asilo.Core.Entities.Administrativo;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _service;
        private readonly IUsuarioRepository _repo;

        public UsuariosController(UsuarioService service, IUsuarioRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            var usuario = _service.Authenticate(username, password);
            if (usuario == null) return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                mensaje = "Login exitoso",
                usuario = usuario.Username,
                rol = usuario.RolId
            });
        }

        [HttpPost("registro")]
        public IActionResult Registrar(string username, string password, int rolId)
        {
            try
            {
                _service.CrearUsuario(username, password, rolId, "CoreAPI");
                return Ok(new { mensaje = "Usuario creado" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var u = _repo.GetById(id);
            if (u == null) return NotFound();
            u.PasswordHash = ""; // Seguridad: no devolvemos el hash
            return Ok(u);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest("ID mismatch");
            _repo.Update(usuario);
            return Ok(new { mensaje = "Usuario actualizado" });
        }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            _repo.Delete(id);
            return Ok(new { mensaje = "Usuario eliminado" });
        }
    }
}