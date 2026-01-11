using Microsoft.AspNetCore.Mvc;
using Asilo.Core.Entities.Financiero;
using Asilo.Core.Enums;
using Asilo.Core.Services;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly CuentaService _service;
        private readonly IMovimientoRepository _movRepo;

        public TransaccionesController(CuentaService service, IMovimientoRepository movRepo)
        {
            _service = service;
            _movRepo = movRepo;
        }

        [HttpPost("cobrar")]
        public IActionResult Cobrar([FromBody] Movimiento movimiento)
        {
            try
            {
                if (movimiento.Tipo == TipoMovimiento.Pago)
                {
                    _service.PagarCuenta(movimiento.CuentaId, movimiento.Monto, "CoreAPI", movimiento.Detalles);
                }
                else
                {
                    _service.AumentarCuenta(movimiento.CuentaId, movimiento.Monto, "CoreAPI", movimiento.Detalles);
                }
                return Ok(new { mensaje = "Transacción exitosa" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_movRepo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var mov = _movRepo.GetById(id);
            if (mov == null) return NotFound();
            return Ok(mov);
        }
    }
}