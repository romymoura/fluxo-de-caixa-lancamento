namespace FluxoDeCaixa.Lancamento.Common.Configuration;

/// <summary>
/// Configuração do publicador SQS
/// </summary>
public class SqsPublisherConfig
{
    /// <summary>
    /// URL da fila SQS
    /// </summary>
    public string QueueUrl { get; set; }

    /// <summary>
    /// Região AWS
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Chave de acesso AWS (opcional se usar instância EC2 com IAM)
    /// </summary>
    public string AccessKey { get; set; }

    /// <summary>
    /// Chave secreta AWS (opcional se usar instância EC2 com IAM)
    /// </summary>
    public string SecretKey { get; set; }
}