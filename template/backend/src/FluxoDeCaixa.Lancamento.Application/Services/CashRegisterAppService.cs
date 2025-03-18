using AutoMapper;
using CSharpFunctionalExtensions;
using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using FluxoDeCaixa.Lancamento.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FluxoDeCaixa.Lancamento.Application.Services;

public class CashRegisterAppService : BaseService<CashRegisterAppService>, ICashRegisterAppService
{
    private readonly ISqsPublisher _sqsPublisher;
    private readonly ICasheRegisterRepository _casheRegisterRepository;
    private readonly IStoreRepository _storeRepository;
    public CashRegisterAppService(
        ISqsPublisher sqsPublisher,
        IMapper mapper,
        ICasheRegisterRepository casheRegisterRepository,
        IStoreRepository storeRepository,
        ILogger<CashRegisterAppService> logger) : base(mapper, logger)
    {
        _sqsPublisher = sqsPublisher;
        _casheRegisterRepository = casheRegisterRepository;
        _storeRepository = storeRepository;
    }

    /// <summary>
    /// Envio de mensagem padrão
    //  Para filas FIFO
    //  await _sqsPublisher.PublishMessageAsync(minhaEntidade, messageGroupId: "grupo-1", messageDeduplicationId: Guid.NewGuid().ToString());
    //  Mensagem com atraso (delay)
    //  await _sqsPublisher.PublishDelayedMessageAsync(minhaEntidade, delaySeconds: 60);
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<CashRegisterResponse>> SendCashRegisterMessageAsync(CashRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Inicio processamento para caixa registradora regra de negocio, mensagria");
            var store = await StoreExists(request.StoreId, cancellationToken);
            var requestJson = JsonConvert.SerializeObject(request);
            var resultService = await _sqsPublisher.PublishMessageAsync(requestJson, cancellationToken: cancellationToken);
            var result = _mapper.Map<CashRegisterResponse>(resultService);
            _logger.LogInformation("Fim processamento para caixa registradora regra de negocio, mensagria");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no processamento para caixa registradora regra de negocio, mensagria");
            return Result.Failure<CashRegisterResponse>(ex.Message);
        }
    }


    public async Task<Result<CashRegisterRepoResponse>> SendCashRegisterPersistenceAsync(CashRegisterRepoRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Inicio processamento para caixa registradora regra de negocio, persistencia");
            var product = _mapper.Map<Product>(request);
            var store = await StoreExists(product.StoreId, cancellationToken);
            product.Store = store;
            var resultRepo = await _casheRegisterRepository.CreateAsync<Guid>(product, cancellationToken);
            var result = _mapper.Map<CashRegisterRepoResponse>(resultRepo);
            _logger.LogInformation("Fim processamento para caixa registradora regra de negocio, persistencia");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no processamento para caixa registradora regra de negocio, persistencia");
            return Result.Failure<CashRegisterRepoResponse>(ex.Message);
        }
    }


    private async Task<Store> StoreExists(Guid? idStore, CancellationToken cancellationToken = default)
    {
        if(idStore == null)
            throw new Exception("Identificador da loja não é válido!");
        var store = await _storeRepository.GetByIdAsync(idStore.Value, cancellationToken);
        if (store == null)
            throw new Exception("Loja não encontrada em nosso sistema");
        return store;
    }
}
