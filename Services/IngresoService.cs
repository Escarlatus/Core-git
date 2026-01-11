using Asilo.Core.Entities.Medico;
using Asilo.Core.Enums;
using Asilo.Core.Interfaces;

namespace Asilo.Core.Services
{
    public class IngresoService
    {
        private readonly IIngresoRepository _repo;
        private readonly IHabitacionRepository _habRepo;

        public IngresoService(IIngresoRepository repo, IHabitacionRepository habRepo)
        {
            _repo = repo;
            _habRepo = habRepo;
        }

        public void RegistrarIngreso(Ingreso nuevo)
        {
            // 1. Validar que no tenga ya un ingreso activo
            var activo = _repo.GetIngresoActivo(nuevo.ResidenteId);
            if (activo != null)
                throw new Exception("El residente ya tiene un ingreso activo.");

            // 2. Validar habitación
            if (nuevo.HabitacionId.HasValue)
            {
                // Aquí podrías validar capacidad si quisieras
            }

            // 3. Forzar estado inicial
            nuevo.Estado = EstadoIngreso.Activo; // <--- USAMOS EL ENUM
            nuevo.FechaIngreso = DateTime.Now;

            _repo.Add(nuevo);
        }

        public void DarAlta(int ingresoId)
        {
            var ingreso = _repo.GetById(ingresoId);
            if (ingreso == null) throw new Exception("Ingreso no encontrado");

            if (ingreso.Estado != EstadoIngreso.Activo)
                throw new Exception("El paciente ya fue dado de alta previamente.");

            // CAMBIO IMPORTANTE AQUÍ:
            ingreso.Estado = EstadoIngreso.AltaMedica; // <--- USAMOS EL NUEVO ESTADO
            ingreso.FechaAlta = DateTime.Now;

            // Al dar de alta, liberamos la habitación (opcional, poner null)
            // ingreso.HabitacionId = null; 

            _repo.Update(ingreso);
        }
    }
}