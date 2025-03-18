using Amazon.SQS.Model;

namespace FluxoDeCaixa.Lancamento.Domain.Services;

public interface ISqsPublisher
{
    /// <summary>
    /// Publica uma mensagem na fila SQS
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser serializado e enviado</typeparam>
    /// <param name="message">Mensagem a ser enviada</param>
    /// <param name="messageGroupId">ID do grupo de mensagens (obrigatório para filas FIFO)</param>
    /// <param name="messageDeduplicationId">ID de deduplicação (obrigatório para filas FIFO)</param>
    /// <returns>Resposta do SQS contendo o MessageId</returns>
    Task<SendMessageResponse> PublishMessageAsync<T>(T message, string messageGroupId = null, string messageDeduplicationId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publica uma mensagem com atraso na fila SQS
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser serializado e enviado</typeparam>
    /// <param name="message">Mensagem a ser enviada</param>
    /// <param name="delaySeconds">Tempo de atraso em segundos (0-900)</param>
    /// <param name="messageGroupId">ID do grupo de mensagens (obrigatório para filas FIFO)</param>
    /// <param name="messageDeduplicationId">ID de deduplicação (obrigatório para filas FIFO)</param>
    /// <returns>Resposta do SQS contendo o MessageId</returns>
    Task<SendMessageResponse> PublishDelayedMessageAsync<T>(T message, int delaySeconds, string messageGroupId = null, string messageDeduplicationId = null,
        CancellationToken cancellationToken = default);
}
