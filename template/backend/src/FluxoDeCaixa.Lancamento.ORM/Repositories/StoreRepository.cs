using FluxoDeCaixa.Lancamento.Domain.Entities;
using FluxoDeCaixa.Lancamento.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.ORM.Repositories;

public class StoreRepository : BaseRepository<StoreRepository, Store>, IStoreRepository
{
    public StoreRepository(DefaultContext context, ILogger<StoreRepository> logger) : base(context, logger) { }
}
