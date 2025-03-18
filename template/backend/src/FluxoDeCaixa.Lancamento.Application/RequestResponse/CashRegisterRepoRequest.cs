using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Application.RequestResponse;

public class CashRegisterRepoRequest
{
    public string? IdStore { get; set; }
    public string? IdMessage { get; set; }
    public DateTime? CreateDate { get; set; }
    public decimal? Price { get; set; } = 0;
    public int? Amount { get; set; } = 1;
    public decimal? Total
    {
        get
        {
            return Price * Amount;
        }
    }
    public CashRegisterType CashRegisterType { get; set; }
}
