using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionesController : ControllerBase
    {
        private readonly IHabitacionRepository _repo;

        public HabitacionesController(IHabitacionRepository repo) => _repo = repo;

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("disponibles")]
        public IActionResult GetDisponibles() => Ok(_repo.GetDisponibles());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var h = _repo.GetById(id);
            return h == null ? NotFound() : Ok(h);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Habitacion h)
        {
            _repo.Add(h);
            return Ok(new { mensaje = "Habitación creada" });
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Habitacion h)
        {
            if (id != h.Id) return BadRequest("ID mismatch");
            _repo.Update(h);
            return Ok(new { mensaje = "Habitación actualizada" });
        }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            _repo.Delete(id);
            return Ok(new { mensaje = "Habitación eliminada" });
        }
    }
}