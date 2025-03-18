using Microsoft.AspNetCore.Builder;

namespace FluxoDeCaixa.Lancamento.IoC;
public interface IModuleInitializer
{
    void Initialize(WebApplicationBuilder builder);
}
