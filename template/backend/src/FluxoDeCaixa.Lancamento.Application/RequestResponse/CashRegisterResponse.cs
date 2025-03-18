using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Application.RequestResponse;

public class CashRegisterResponse
{
    public Guid StoreId { get; set; }
    public Guid ProductId { get; set; }
    public Guid MessageId { get; set; }
    public CashRegisterType CashRegisterType { get; set; }
}
