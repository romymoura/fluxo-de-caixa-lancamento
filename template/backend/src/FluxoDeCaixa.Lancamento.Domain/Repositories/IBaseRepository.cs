namespace FluxoDeCaixa.Lancamento.Domain.Repositories;
public interface IBaseRepository<TEntity>
{
    Task<TEntity> CreateAsync<TKey>(TEntity item, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync<TKey>(TEntity item, CancellationToken cancellationToken = default);
}
