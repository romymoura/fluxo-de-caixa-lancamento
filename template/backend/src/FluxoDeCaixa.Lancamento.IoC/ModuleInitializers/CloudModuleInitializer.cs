using FluxoDeCaixa.Lancamento.Cloud.AWS;
using FluxoDeCaixa.Lancamento.Common.Configuration;
using FluxoDeCaixa.Lancamento.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Lancamento.IoC.ModuleInitializers;

public class CloudModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.Configure<SqsPublisherConfig>(builder.Configuration.GetSection("SqsPublisher"));
        RegisterDependencies(ref builder);
    }

    private void RegisterDependencies(ref WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ISqsPublisher, SqsPublisher>();
    }
}

