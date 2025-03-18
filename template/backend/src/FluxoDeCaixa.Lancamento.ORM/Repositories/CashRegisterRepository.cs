using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.ORM.Repositories;

public class CashRegisterRepository : BaseRepository<CashRegisterRepository>, ICasheRegisterRepository
{
    public CashRegisterRepository(DefaultContext context, ILogger<CashRegisterRepository> logger) : base(context, logger) { }

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Inicio processamento para caixa registradora repository");
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Fim processamento para caixa registradora repository");
        return product;
    }
}
