using Asilo.Core.Enums; 

namespace Asilo.Core.Entities.Financiero
{
    public class Movimiento
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public TipoMovimiento Tipo { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public bool Sincronizado { get; set; } 
    }
}