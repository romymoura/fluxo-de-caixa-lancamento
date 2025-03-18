using AutoMapper;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Common.Enums;
using FluxoDeCaixa.Lancamento.Domain.Entities;

namespace FluxoDeCaixa.Lancamento.Application.MappingProfile;

public class CashRegisterProfile : Profile
{
    public CashRegisterProfile()
    {
        // Response da Cloud para Application, para retorno do contrato.
        CreateMap<Amazon.SQS.Model.SendMessageResponse, RequestResponse.CashRegisterResponse>()
            .BeforeMap((source, destination) =>
            {
                destination.IdMessage = new Guid(source.MessageId);
            });

        // Request da Application para Repo.
        CreateMap<CashRegisterRepoRequest, Product>()
            .BeforeMap((source, destination) =>
            {
                destination.Id = Guid.NewGuid(); // informa o novo registro do produto
                destination.IdStore = new Guid(source.IdStore ?? Guid.NewGuid().ToString());
                destination.IdMessage = new Guid(source.IdMessage ?? Guid.NewGuid().ToString());
                destination.CreatedAt = source.CreateDate ?? DateTime.Now;
                destination.Description = source.CashRegisterType switch
                {
                    CashRegisterType.Debit => "Débito registrado com sucesso!",
                    CashRegisterType.Credit => "Crédito registrado com sucesso!",
                    _ => "Registro sem identificação!"
                };
                destination.Amount = source.Amount ?? 1;
            });

        // Response da Repo para Application.
        CreateMap<Product, CashRegisterRepoResponse>()
            .BeforeMap((source, destination) =>
            {
                //destination.Persitence = true;
            });
    }
}

