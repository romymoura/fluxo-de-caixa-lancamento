using AutoMapper;

namespace FluxoDeCaixa.Lancamento.WebApi.Utils.MappingProfile;

public class CashRegisterProfile : Profile
{
    public CashRegisterProfile()
    {
        // Request da ui para application
        CreateMap<RequestResponse.CashRegisterRequestResponse, Application.RequestResponse.CashRegisterRequest>()
            .BeforeMap((source, destination) =>
            {
                destination.Amount = source.Amount;
                destination.Price = source.Price;
                destination.CashRegisterType = source.CashRegisterType;
            });

        // Response da application para ui, para retorno do contrato.
        CreateMap<Application.RequestResponse.CashRegisterResponse, RequestResponse.CashRegisterResponseRequest>()
            .BeforeMap((source, destination) =>
            {
                destination.StoreId = source.StoreId.ToString();
                destination.ProductId = source.ProductId.ToString();
                destination.MessageId = source.MessageId.ToString();
                destination.CashRegisterType = source.CashRegisterType.ToString();
            });

        // Response da application para ui, para retorno do contrato.
        CreateMap<Application.RequestResponse.CashRegisterRepoRequest, RequestResponse.CashRegisterResponseRequest>()
            .BeforeMap((source, destination) =>
            {
                destination.StoreId = source.StoreId;
                destination.ProductId = source.ProductId;
                destination.MessageId = source.MessageId;
                destination.CashRegisterType = source.CashRegisterType.ToString();
            });
    }
}
