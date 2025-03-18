using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Application.RequestResponse;

public class CashRegisterRequest
{
    public CashRegisterType CashRegisterType { get; set; }
    public decimal? Price { get; set; }
    public Guid? IdStore { get; set; }
}