using FluxoDeCaixa.Lancamento.Application;
using FluxoDeCaixa.Lancamento.Common.Configuration;
using FluxoDeCaixa.Lancamento.Common.Logging;
using FluxoDeCaixa.Lancamento.Common.Security;
using FluxoDeCaixa.Lancamento.IoC.ModuleInitializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Claims;


namespace FluxoDeCaixa.Lancamento.IoC;
public static class StartModules
{
    public static void InitializerModulesNow(this WebApplicationBuilder builder)
    {
        builder.InitLogger();
        builder.InitConfigSwagger();
        builder.InitSecurity();
        builder.InitAutoMapper();

        new WebApiModuleInitializer().Initialize(builder);
        new ApplicationModuleInitializer().Initialize(builder);
        new InfrastructureModuleInitializer().Initialize(builder);
        new CloudModuleInitializer().Initialize(builder);
    }

    private static void InitLogger(this WebApplicationBuilder builder)
    {
        builder.AddDefaultLogger();
    }

    private static void InitConfigSwagger(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            string caminhoAplicacao = AppDomain.CurrentDomain.BaseDirectory;
            string nomeAplicacao = AppDomain.CurrentDomain.FriendlyName;
            string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
            c.IncludeXmlComments(caminhoXmlDoc);
        });
    }

    private static void InitSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SecurityConfig>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddJwtAuthentication(builder.Configuration);

        // Authorization service, policy
        builder.Services.AddAuthorization(options =>
        {
            /*Exemplos pegando a claim para orquestrar o authorization*/

#if Release
            options.AddPolicy("OnlyStore", policy => policy.RequireAssertion(h =>
                h.User.FindFirstValue(ClaimTypes.Role).Contains("Store"))
            );
            options.AddPolicy("OnlyAdmin", policy => policy.RequireAssertion(h =>
                h.User.FindFirstValue(ClaimTypes.Role).Contains("Admin"))
            );
            options.AddPolicy("OnlyCustomer", policy => policy.RequireAssertion(h =>
                h.User.FindFirstValue(ClaimTypes.Role).Contains("Customer"))
            );
#endif            

        });
    }

    private static void InitAutoMapper(this WebApplicationBuilder builder)
    {
        Assembly apiAssembly = Assembly.Load("FluxoDeCaixa.Lancamento.WebApi");

        builder.Services.AddAutoMapper(apiAssembly, typeof(ApplicationLayer).Assembly);
    }
}
