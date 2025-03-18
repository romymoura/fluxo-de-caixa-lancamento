using FluxoDeCaixa.Lancamento.Application.Interfaces;
using FluxoDeCaixa.Lancamento.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Lancamento.IoC.ModuleInitializers;
public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICashRegisterAppService, CashRegisterAppService>();
    }


}