using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedimientosController : ControllerBase
    {
        private readonly IProcedimientoRepository _repo;
        public ProcedimientosController(IProcedimientoRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("buscar")]
        public IActionResult Buscar([FromQuery] string nombre) => Ok(_repo.BuscarPorNombre(nombre));

        [HttpGet("{id}")]
        public IActionResult GetById(int id) => _repo.GetById(id) is var p && p != null ? Ok(p) : NotFound();

        [HttpPost]
        public IActionResult Crear([FromBody] Procedimiento p) { _repo.Add(p); return Ok(); }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Procedimiento p) { _repo.Update(p); return Ok(); }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id) { _repo.Delete(id); return Ok(); }
    }
}