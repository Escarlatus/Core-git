using Asilo.Core.Entities.Medico;
using Asilo.Core.Entities.Financiero;
using Asilo.Core.Interfaces;
using Asilo.Core.Rules;

namespace Asilo.Core.Services;

public class ResidenteService
{
    private readonly IResidenteRepository _resRepo;
    private readonly ICuentaRepository _cuentaRepo;
    private readonly AuditService _audit;

    public ResidenteService(IResidenteRepository resRepo, ICuentaRepository cuentaRepo, AuditService audit)
    {
        _resRepo = resRepo;
        _cuentaRepo = cuentaRepo;
        _audit = audit;
    }

    public Residente CrearResidente(Residente r, string realizadoPor)
    {
        // validaciones básicas
        if (string.IsNullOrWhiteSpace(r.Nombre))
            throw new ArgumentException("Nombre requerido");
        if (string.IsNullOrWhiteSpace(r.Documento))
            throw new ArgumentException("Documento requerido");

        var existente = _resRepo.GetByDocumento(r.Documento);
        if (existente != null) throw new InvalidOperationException("Residente ya existe");

        _resRepo.Add(r);

        // crear cuenta asociada con balance cero
        var cuenta = new Cuenta { ResidenteId = r.Id, Balance = 0m };
        _cuentaRepo.Add(cuenta);

        _audit.Log(realizadoPor, "CREAR_RESIDENTE", $"Residente {r.Documento} creado y cuenta {cuenta.Id}");

        return r;
    }

    public void AsignarHabitacion(int residenteId, int habitacionId, string realizadoPor)
    {
        var r = _resRepo.GetById(residenteId) ?? throw new Exception("Residente no encontrado");
        r.HabitacionId = habitacionId;
        _resRepo.Update(r);
        _audit.Log(realizadoPor, "ASIGNAR_HABITACION", $"Residente {residenteId} -> Hab {habitacionId}");
    }
}