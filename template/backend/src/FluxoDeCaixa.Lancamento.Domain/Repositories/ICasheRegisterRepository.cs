using FluxoDeCaixa.Lancamento.Domain.Entities;

namespace FluxoDeCaixa.Lancamento.Domain.Repositories;

public interface ICasheRegisterRepository
{
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
}
