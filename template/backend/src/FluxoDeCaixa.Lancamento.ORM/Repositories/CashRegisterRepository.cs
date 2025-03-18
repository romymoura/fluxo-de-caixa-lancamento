using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.ORM.Repositories;

public class CashRegisterRepository : BaseRepository<CashRegisterRepository, Product>, ICasheRegisterRepository
{
    public CashRegisterRepository(DefaultContext context, ILogger<CashRegisterRepository> logger) : base(context, logger) { }
    //public override async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    //{
    //    //_logger.LogInformation("Inicio processamento para caixa registradora repository");
    //    //var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == product.StoreId);
    //    //if (store == null)
    //    //    throw new Exception("Loja não cadastrada em nosso sistema");
    //    //product.Store = store;
    //    //await _context.Products.AddAsync(product, cancellationToken);
    //    //await _context.SaveChangesAsync(cancellationToken);
    //    //_logger.LogInformation("Fim processamento para caixa registradora repository");
    //    //return product;
    //    var result = await base.CreateAsync(product, cancellationToken);
    //    return result;
    //}
}
