using CSharpFunctionalExtensions;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;

namespace FluxoDeCaixa.Lancamento.Application.Interfaces;

public interface ICashRegisterAppService
{
    Task<Result<CashRegisterResponse>> SendCashRegisterAsync(CashRegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<CashRegisterRepoResponse>> SendCashRegisterPersistenceAsync(CashRegisterRepoRequest request, CancellationToken cancellationToken = default);
}
