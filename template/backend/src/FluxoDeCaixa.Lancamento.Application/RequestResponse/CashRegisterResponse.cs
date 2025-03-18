using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Application.RequestResponse;

public class CashRegisterResponse
{
    public Guid IdStore { get; set; }
    public Guid IdProduct { get; set; }
    public Guid IdMessage { get; set; }
    public CashRegisterType Status { get; set; }
}
