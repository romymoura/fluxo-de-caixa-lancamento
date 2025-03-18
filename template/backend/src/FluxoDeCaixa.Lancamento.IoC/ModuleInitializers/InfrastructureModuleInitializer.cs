using FluxoDeCaixa.Lancamento.ORM;
using FluxoDeCaixa.Lancamento.ORM.Repositories;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Lancamento.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("FluxoDeCaixa.Lancamento.ORM")
                )
            );
        RegisterDependencies(ref builder);
    }


    private void RegisterDependencies(ref WebApplicationBuilder builder)
    {
        //builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        builder.Services.AddScoped<ICasheRegisterRepository, CashRegisterRepository>();
        builder.Services.AddScoped<IStoreRepository, StoreRepository>();
    }
}
