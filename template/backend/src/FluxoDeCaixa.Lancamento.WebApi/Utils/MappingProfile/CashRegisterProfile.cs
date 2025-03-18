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
                destination.Price = source.Price;
                destination.CashRegisterType = source.CashRegisterType;
            });

        // Response da application para ui, para retorno do contrato.
        CreateMap<Application.RequestResponse.CashRegisterResponse, RequestResponse.CashRegisterResponseRequest>()
            .BeforeMap((source, destination) =>
            {
                destination.Id = source.IdProduct.ToString();
            });
    }
}
