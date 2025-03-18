using Bogus;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Common.Enums;

namespace FluxoDeCaixa.Lancamento.Unit.Application.SourceData;

public static class CashRegisterAppServiceSourceData
{
    public static readonly Faker<CashRegisterRequest> CashRegisterRequestFaker = new Faker<CashRegisterRequest>()
       .RuleFor(p => p.StoreId, f => f.Random.Guid())
       .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
       .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
       .RuleFor(x => x.CashRegisterType, f => CashRegisterType.Credit);


    public static readonly Faker<CashRegisterRepoRequest> CashRegisterRequestPersistenceFaker = new Faker<CashRegisterRepoRequest>()
      .RuleFor(p => p.StoreId, f => Guid.NewGuid().ToString())
      .RuleFor(p => p.MessageId, f => Guid.NewGuid().ToString())
      .RuleFor(p => p.CreateDate, f => DateTime.UtcNow)
      .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
      .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
      .RuleFor(x => x.CashRegisterType, f => CashRegisterType.Credit);
}
