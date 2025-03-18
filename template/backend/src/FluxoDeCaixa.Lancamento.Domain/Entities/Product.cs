using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Domain.Entities;
public class Product : BaseEntity
{
    public Guid IdStore { get; set; }
    public Guid IdMessage { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public CashRegisterType CashRegisterType { get; set; }
    public Store Store { get; set; }
}
