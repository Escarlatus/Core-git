using Asilo.Core.Entities.Financiero;
using Asilo.Core.Interfaces;
using Asilo.Core.Enums;
namespace Asilo.Core.Services;

public class CuentaService
{
    private readonly ICuentaRepository _cuentaRepo;
    private readonly IMovimientoRepository _movRepo;
    private readonly AuditService _audit;

    public CuentaService(ICuentaRepository cuentaRepo, IMovimientoRepository movRepo, AuditService audit)
    {
        _cuentaRepo = cuentaRepo;
        _movRepo = movRepo;
        _audit = audit;
    }

    public void AumentarCuenta(int cuentaId, decimal monto, string realizadoPor, string detalle = "")
    {
        if (monto <= 0) throw new ArgumentException("Monto debe ser mayor que cero");
        var cuenta = _cuentaRepo.GetById(cuentaId) ?? throw new Exception("Cuenta no encontrada");
        cuenta.Balance += monto;
        _cuentaRepo.Update(cuenta);

        var mov = new Movimiento { CuentaId = cuentaId, Monto = monto, Fecha = DateTime.UtcNow, Tipo = TipoMovimiento.Cargo, Detalles = detalle };
        _movRepo.Add(mov);

        _audit.Log(realizadoPor, "AUMENTAR_CUENTA", $"Cuenta {cuentaId} monto {monto}");
    }

    public void PagarCuenta(int cuentaId, decimal monto, string realizadoPor, string detalle = "")
    {
        if (monto <= 0) throw new ArgumentException("Monto debe ser mayor que cero");
        var cuenta = _cuentaRepo.GetById(cuentaId) ?? throw new Exception("Cuenta no encontrada");
        cuenta.Balance -= monto;
        _cuentaRepo.Update(cuenta);

        var mov = new Movimiento { CuentaId = cuentaId, Monto = monto, Fecha = DateTime.UtcNow, Tipo = TipoMovimiento.Pago, Detalles = detalle };
        _movRepo.Add(mov);

        _audit.Log(realizadoPor, "PAGAR_CUENTA", $"Cuenta {cuentaId} pago {monto}");
    }
}