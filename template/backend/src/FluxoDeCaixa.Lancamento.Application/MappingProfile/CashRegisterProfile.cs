﻿using AutoMapper;
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
                destination.MessageId = new Guid(source.MessageId);
            });

        // Request da Application para Repo.
        CreateMap<CashRegisterRepoRequest, Product>()
            .BeforeMap((source, destination) =>
            {
                destination.StoreId = new Guid(source.StoreId ?? Guid.NewGuid().ToString());
                destination.MessageId = new Guid(source.MessageId ?? Guid.NewGuid().ToString());
                destination.CreatedAt = source.CreateDate ?? DateTime.Now;
                destination.Description = source.CashRegisterType switch
                {
                    CashRegisterType.Debit => "Débito registrado com sucesso!",
                    CashRegisterType.Credit => "Crédito registrado com sucesso!",
                    _ => "Registro sem identificação!"
                };
                destination.Amount = source.Amount ?? 1;
                destination.Price = source.Price ?? 0;
                destination.Subtotal = source.Subtotal ?? 0;
            });

        // Response da Repo para Application.
        CreateMap<Product, CashRegisterRepoResponse>()
            .BeforeMap((source, destination) =>
            {
                destination.ProductId = source.Id;
            });
    }
}