using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        private readonly IServicioRepository _repo;
        public ServiciosController(IServicioRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => _repo.GetById(id) is var s && s != null ? Ok(s) : NotFound();

        [HttpPost]
        public IActionResult Crear([FromBody] Servicio s) { _repo.Add(s); return Ok(); }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Servicio s) { _repo.Update(s); return Ok(); }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id) { _repo.Delete(id); return Ok(); }
    }
}