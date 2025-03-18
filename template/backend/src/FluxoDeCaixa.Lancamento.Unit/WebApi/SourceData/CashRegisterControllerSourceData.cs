using Bogus;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Common.Enums;
using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.WebApi.Utils.RequestResponse;

namespace FluxoDeCaixa.Lancamento.Unit.WebApi.SourceData;

internal static class CashRegisterControllerSourceData
{

    #region Arrange Cashe Register
    public static (
        CashRegisterRequestResponse request,
        CashRegisterRequest cashRegisterMessageRequest,
        CashRegisterResponse cashRegisterResponse,
        CashRegisterRepoRequest cashRegisterRepoRequest,
        CashRegisterRepoResponse cashRegisterRepoResponse,
        CashRegisterResponseRequest cashRegisterResponseRequest) ArrangeCasheRegisterCredit()
    {
        var request = CashRegisterControllerSourceData.CasheRegisterCreditFaker;
        var cashRegisterMessageRequest = new CashRegisterRequest
        {
            StoreId = Guid.Parse("862e08eb-fa5b-4393-9e82-983002538378"),
            Amount = request.Amount,
            Price = request.Price,
            CashRegisterType = request.CashRegisterType
        };
        var cashRegisterResponse = new CashRegisterResponse
        {
            MessageId = Guid.NewGuid()
        };
        var cashRegisterRepoRequest = new CashRegisterRepoRequest
        {
            StoreId = "862e08eb-fa5b-4393-9e82-983002538378",
            MessageId = cashRegisterResponse.MessageId.ToString(),
            CreateDate = DateTime.UtcNow,
            Amount = request.Amount,
            Price = request.Price,
            CashRegisterType = request.CashRegisterType
        };

        var cashRegisterRepoResponse = new CashRegisterRepoResponse
        {
            ProductId = Guid.NewGuid()
        };

        var cashRegisterResponseRequest = new CashRegisterResponseRequest
        {
            MessageId = cashRegisterResponse.MessageId.ToString(),
            ProductId = cashRegisterRepoResponse.ProductId.ToString()
        };

        return (
            request,
            cashRegisterMessageRequest,
            cashRegisterResponse,
            cashRegisterRepoRequest,
            cashRegisterRepoResponse,
            cashRegisterResponseRequest
         );
    }
    #endregion


    private static readonly Faker<CashRegisterRequest> CashRegisterRequestFaker = new Faker<CashRegisterRequest>()
        .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.CashRegisterType, f => CashRegisterType.Credit);



    private static readonly Faker<CashRegisterRequestResponse> CashRegisterRequestResponseCreditFaker = new Faker<CashRegisterRequestResponse>()
        .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.CashRegisterType, f => CashRegisterType.Credit);
    public static CashRegisterRequestResponse CasheRegisterCreditFaker => CashRegisterRequestResponseCreditFaker.Generate();

    private static readonly Faker<CashRegisterRequestResponse> CashRegisterRequestResponseDebitFaker = new Faker<CashRegisterRequestResponse>()
        .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
        .RuleFor(x => x.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(x => x.CashRegisterType, f => CashRegisterType.Credit);
    public static CashRegisterRequestResponse CasheRegisterDebitFaker => CashRegisterRequestResponseDebitFaker.Generate();


    private static readonly Faker<Product> ProductValidFaker = new Faker<Product>()
        .RuleFor(p => p.MessageId, f => f.Random.Guid())
        .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
        .RuleFor(p => p.Amount, f => f.Random.Int(1, 100))
        .RuleFor(p => p.Subtotal, (f, p) => p.Price * p.Amount)
        .RuleFor(p => p.Description, f => f.Commerce.ProductName())
        .RuleFor(p => p.CashRegisterType, f => f.PickRandom<CashRegisterType>())
        .RuleFor(p => p.StoreId, f => Guid.Parse("862e08eb-fa5b-4393-9e82-983002538378"));

    private static readonly Faker<Product> ProductInvalidFaker = new Faker<Product>()
       .RuleFor(p => p.MessageId, f => f.Random.Guid())
       .RuleFor(p => p.Price, f => f.Random.Decimal(1, 1000))
       .RuleFor(p => p.Amount, f => f.Random.Int(1, 100))
       .RuleFor(p => p.Subtotal, (f, p) => p.Price * p.Amount)
       .RuleFor(p => p.Description, f => f.Commerce.ProductName())
       .RuleFor(p => p.CashRegisterType, f => f.PickRandom<CashRegisterType>())
       .RuleFor(p => p.StoreId, f => f.Random.Guid());





    public static Product GenerateValidProduct()
    {
        return ProductValidFaker.Generate();
    }
    public static Product GenerateInvalidProduct()
    {
        return ProductInvalidFaker.Generate();
    }
}
