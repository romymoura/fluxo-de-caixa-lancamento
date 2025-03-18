namespace FluxoDeCaixa.Lancamento.Common.Configuration;

public class SecurityConfig
{
    public string? IssuerKey { get; set; }
    public string? TokenAudirence { get; set; }
    public string? Issuer { get; set; }
    public int TokenExp { get; set; }
}
