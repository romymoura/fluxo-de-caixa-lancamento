using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Application.RequestResponse;

public class CashRegisterRequest
{
    public CashRegisterType CashRegisterType { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Amount { get; set; } = 1;
    public decimal? Subtotal
    {
        get
        {
            return Price * (CashRegisterType == CashRegisterType.Credit ? Amount : 1);
        }
    }
    public Guid? StoreId { get; set; }
}