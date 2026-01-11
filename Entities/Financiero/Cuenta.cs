namespace Asilo.Core.Entities.Financiero;

public class Cuenta
{
    public int Id { get; set; }
    public int ResidenteId { get; set; }
    public decimal Balance { get; set; } = 0m;
}