using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalisisController : ControllerBase
    {
        private readonly IAnalisisRepository _repo;
        public AnalisisController(IAnalisisRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => _repo.GetById(id) is var a && a != null ? Ok(a) : NotFound();

        [HttpPost]
        public IActionResult Crear([FromBody] Analisis a) { _repo.Add(a); return Ok(); }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Analisis a) { _repo.Update(a); return Ok(); }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id) { _repo.Delete(id); return Ok(); }
    }
}