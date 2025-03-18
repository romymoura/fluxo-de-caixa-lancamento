using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Domain.Entities;
public class Product : BaseEntity
{
    public Guid MessageId { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    public string Description { get; set; } = string.Empty;
    public CashRegisterType CashRegisterType { get; set; }
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = new Store();
}
