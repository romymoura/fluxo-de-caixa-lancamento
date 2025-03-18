using AutoMapper;
using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Application.RequestResponse;
using FluxoDeCaixa.Lancamento.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CSharpFunctionalExtensions;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using FluxoDeCaixa.Lancamento.Domain.Entities;

namespace FluxoDeCaixa.Lancamento.Application.Services;

public class CashRegisterAppService : BaseService<CashRegisterAppService>, ICashRegisterAppService
{
    private readonly ISqsPublisher _sqsPublisher;
    private readonly ICasheRegisterRepository _casheRegisterRepository;
    public CashRegisterAppService(
        ISqsPublisher sqsPublisher,
        IMapper mapper,
        ICasheRegisterRepository casheRegisterRepository,
        ILogger<CashRegisterAppService> logger) : base(mapper, logger)
    {
        _sqsPublisher = sqsPublisher;
        _casheRegisterRepository = casheRegisterRepository;
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
    public async Task<Result<CashRegisterResponse>> SendCashRegisterAsync(CashRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Inicio processamento para caixa registradora regra de negocio, mensagria");
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
            var resultRepo = await _casheRegisterRepository.CreateAsync(product, cancellationToken);
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
}
