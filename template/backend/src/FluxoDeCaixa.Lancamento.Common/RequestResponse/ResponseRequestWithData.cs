namespace FluxoDeCaixa.Lancamento.Common.RequestResponse;

public class ResponseRequestWithData<T> : ResponseRequest
{
    public T? Data { get; set; }
}
