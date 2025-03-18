using FluxoDeCaixa.Lancamento.IoC;
using FluxoDeCaixa.Lancamento.WebApi.Utils.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appsettings = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json";
builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile(appsettings, optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("*") // Permite chamadas do Angular
              .AllowAnyMethod() // Permite qualquer método (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader(); // Permite qualquer cabeçalho
                                 //.AllowCredentials(); // Permite envio de credenciais (cookies, autenticação)
    });
});
builder.InitializerModulesNow();
builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
app.UseMiddleware<InterceptExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
