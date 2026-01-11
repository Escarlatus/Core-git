using Asilo.Core.Entities.Medico;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Services
{
    public class CatalogoMedicoService
    {
        private readonly IProcedimientoRepository _procRepo;
        private readonly IAnalisisRepository _analisisRepo;

        public CatalogoMedicoService(IProcedimientoRepository procRepo, IAnalisisRepository analisisRepo)
        {
            _procRepo = procRepo;
            _analisisRepo = analisisRepo;
        }

        public void CrearProcedimiento(Procedimiento proc)
        {
            if (proc.Precio < 0) throw new Exception("El precio no puede ser negativo.");
            // Aquí podrías validar si ya existe el nombre
            _procRepo.Add(proc);
        }

        // Métodos similares para Análisis...
    }
}