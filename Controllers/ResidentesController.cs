using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Medico;
using Asilo.Core.Services;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentesController : ControllerBase
    {
        private readonly ResidenteService _service;
        private readonly IResidenteRepository _repo;

        public ResidentesController(ResidenteService service, IResidenteRepository repo)
        {
            _service = service;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var residente = _repo.GetById(id);
            if (residente == null) return NotFound();
            return Ok(residente);
        }

        [HttpGet("buscar/{documento}")]
        public IActionResult GetByDocumento(string documento)
        {
            var residente = _repo.GetByDocumento(documento);
            if (residente == null) return NotFound("No encontrado");
            return Ok(residente);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Residente residente)
        {
            try
            {
                // CoreAPI es el identificador de quien realiza la acción
                var nuevo = _service.CrearResidente(residente, "CoreAPI");
                return Ok(new { mensaje = "Residente creado", id = nuevo.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Residente residente)
        {
            if (id != residente.Id) return BadRequest("El ID de la URL no coincide con el cuerpo");
            try
            {
                _repo.Update(residente);
                return Ok(new { mensaje = "Residente actualizado" });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}/asignar-habitacion/{habitacionId}")]
        public IActionResult AsignarHabitacion(int id, int habitacionId)
        {
            try
            {
                _service.AsignarHabitacion(id, habitacionId, "CoreAPI");
                return Ok(new { mensaje = "Habitación asignada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            try
            {
                _repo.Delete(id);
                return Ok(new { mensaje = "Residente eliminado" });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}