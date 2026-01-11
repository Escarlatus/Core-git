using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Services;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly IngresoService _service;
        private readonly IIngresoRepository _repo;

        public IngresosController(IngresoService service, IIngresoRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpPost("admitir")]
        public IActionResult AdmitirPaciente([FromBody] Ingreso ingreso)
        {
            try
            {
                _service.RegistrarIngreso(ingreso);
                return Ok(new { mensaje = "Paciente ingresado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("alta/{id}")]
        public IActionResult DarAlta(int id)
        {
            try
            {
                _service.DarAlta(id);
                return Ok(new { mensaje = "Paciente dado de alta" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("activo/{residenteId}")]
        public IActionResult GetActivo(int residenteId)
        {
            var ingreso = _repo.GetIngresoActivo(residenteId);
            return ingreso == null ? NotFound() : Ok(ingreso);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var ingreso = _repo.GetById(id);
            if (ingreso == null) return NotFound();
            return Ok(ingreso);
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarManual(int id, [FromBody] Ingreso ingreso)
        {
            if (id != ingreso.Id) return BadRequest("ID mismatch");
            _repo.Update(ingreso);
            return Ok(new { mensaje = "Ingreso actualizado manualmente" });
        }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            _repo.Delete(id);
            return Ok(new { mensaje = "Ingreso eliminado" });
        }
    }
}
