using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Interfaces;
using Asilo.Core.Entities.Financiero;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly IMovimientoRepository _movimientoRepo;

        public CuentasController(ICuentaRepository cuentaRepo, IMovimientoRepository movimientoRepo)
        {
            _cuentaRepo = cuentaRepo;
            _movimientoRepo = movimientoRepo;
        }

        // --- LECTURA (GET) ---

        [HttpGet]
        public IActionResult GetAll() => Ok(_cuentaRepo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cuenta = _cuentaRepo.GetById(id);
            if (cuenta == null) return NotFound();
            return Ok(cuenta);
        }

        [HttpGet("residente/{residenteId}")]
        public IActionResult GetByResidente(int residenteId)
        {
            var cuenta = _cuentaRepo.GetByResidente(residenteId);
            if (cuenta == null) return NotFound(new { mensaje = "El residente no tiene cuenta activa." });
            return Ok(cuenta);
        }

        [HttpGet("{cuentaId}/movimientos")]
        public IActionResult GetMovimientos(int cuentaId)
        {
            var movimientos = _movimientoRepo.GetByCuenta(cuentaId);
            return Ok(movimientos);
        }

        // --- ESCRITURA MANUAL (POST, PUT, DELETE) ---
        // Útiles para correcciones manuales o requisitos académicos

        // POST: Crear una cuenta manualmente (ej. para un residente antiguo)
        [HttpPost]
        public IActionResult Crear([FromBody] Cuenta cuenta)
        {
            try
            {
                _cuentaRepo.Add(cuenta);
                return Ok(new { mensaje = "Cuenta creada manualmente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: Editar el saldo "a la fuerza" (ej. Corrección de error de dedo)
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Cuenta cuenta)
        {
            if (id != cuenta.Id) return BadRequest("El ID de la URL no coincide con el cuerpo");

            try
            {
                _cuentaRepo.Update(cuenta);
                return Ok(new { mensaje = "Cuenta actualizada manualmente" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: Borrar una cuenta (Cuidado: dejará huérfanos a los movimientos)
        [HttpDelete("{id}")]
        public IActionResult Borrar(int id)
        {
            try
            {
                _cuentaRepo.Delete(id);
                return Ok(new { mensaje = "Cuenta eliminada" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}