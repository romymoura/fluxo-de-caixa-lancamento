using FluxoDeCaixa.Lancamento.Common.Validation;

namespace FluxoDeCaixa.Lancamento.Common.RequestResponse;

public class ResponseRequest
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];
}
