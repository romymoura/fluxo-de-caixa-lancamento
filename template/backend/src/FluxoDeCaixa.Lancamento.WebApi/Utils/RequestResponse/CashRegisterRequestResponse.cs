using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.WebApi.Utils.RequestResponse;

public class CashRegisterRequestResponse
{
    public CashRegisterType CashRegisterType { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Amount { get; set; } = 0;
}
