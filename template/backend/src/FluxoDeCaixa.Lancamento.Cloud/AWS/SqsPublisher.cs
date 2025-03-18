using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using FluxoDeCaixa.Lancamento.Common.Configuration;
using FluxoDeCaixa.Lancamento.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace FluxoDeCaixa.Lancamento.Cloud.AWS;

/// <summary>
/// Implementação do serviço de publicação no SQS
/// </summary>
public class SqsPublisher : ISqsPublisher, IDisposable
{
    private readonly IAmazonSQS _sqsClient;
    private readonly SqsPublisherConfig _config;
    private readonly ILogger<SqsPublisher> _logger;
    private bool _disposed = false;

    /// <summary>
    /// Construtor do serviço de publicação no SQS
    /// </summary>
    /// <param name="config">Configuração do publicador</param>
    /// <param name="logger">Logger para registrar informações e erros</param>
    public SqsPublisher(IOptions<SqsPublisherConfig> config, ILogger<SqsPublisher> logger)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(_config.QueueUrl))
            throw new ArgumentException("QueueUrl é obrigatório", nameof(config.Value.QueueUrl));

        if (string.IsNullOrWhiteSpace(_config.Region))
            throw new ArgumentException("Region é obrigatório", nameof(config.Value.Region));

        RegionEndpoint region = RegionEndpoint.GetBySystemName(_config.Region);

        // Se as credenciais forem fornecidas, use-as. Caso contrário, use o provider de credenciais padrão
        _sqsClient = string.IsNullOrEmpty(_config.AccessKey) || string.IsNullOrEmpty(_config.SecretKey)
            ? new AmazonSQSClient(region)
            : new AmazonSQSClient(_config.AccessKey, _config.SecretKey, region);

        _logger.LogInformation("SqsPublisher inicializado para a fila: {QueueUrl}", _config.QueueUrl);
    }

    /// <inheritdoc />
    public async Task<SendMessageResponse> PublishMessageAsync<T>(
        T message,
        string messageGroupId = null,
        string messageDeduplicationId = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogDebug("Mensagem serializada: {Message}", json);

            var request = new SendMessageRequest
            {
                QueueUrl = _config.QueueUrl,
                MessageBody = json
            };

            // Configurações para fila FIFO
            if (!string.IsNullOrEmpty(messageGroupId))
            {
                request.MessageGroupId = messageGroupId;
            }

            if (!string.IsNullOrEmpty(messageDeduplicationId))
            {
                request.MessageDeduplicationId = messageDeduplicationId;
            }

            var response = await _sqsClient.SendMessageAsync(request, cancellationToken);
            _logger.LogInformation("Mensagem enviada com sucesso. MessageId: {MessageId}", response.MessageId);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar mensagem no SQS: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<SendMessageResponse> PublishDelayedMessageAsync<T>(
        T message,
        int delaySeconds,
        string messageGroupId = null,
        string messageDeduplicationId = null,
        CancellationToken cancellationToken = default)
    {
        if (delaySeconds < 0 || delaySeconds > 900)
        {
            throw new ArgumentOutOfRangeException(nameof(delaySeconds), "Atraso deve estar entre 0 e 900 segundos");
        }

        try
        {
            var json = JsonSerializer.Serialize(message);
            _logger.LogDebug("Mensagem serializada: {Message}", json);

            var request = new SendMessageRequest
            {
                QueueUrl = _config.QueueUrl,
                MessageBody = json,
                DelaySeconds = delaySeconds
            };

            // Configurações para fila FIFO
            if (!string.IsNullOrEmpty(messageGroupId))
            {
                request.MessageGroupId = messageGroupId;
            }

            if (!string.IsNullOrEmpty(messageDeduplicationId))
            {
                request.MessageDeduplicationId = messageDeduplicationId;
            }

            var response = await _sqsClient.SendMessageAsync(request, cancellationToken);
            _logger.LogInformation("Mensagem com atraso enviada com sucesso. MessageId: {MessageId}, DelaySeconds: {DelaySeconds}",
                response.MessageId, delaySeconds);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar mensagem com atraso no SQS: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Libera recursos utilizados pelo cliente SQS
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Método protected para implementação do padrão Dispose
    /// </summary>
    /// <param name="disposing">Indica se está liberando recursos gerenciados</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _sqsClient?.Dispose();
        }

        _disposed = true;
    }
}
