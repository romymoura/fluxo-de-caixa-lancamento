using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.ORM.Repositories;

public class BaseRepository<T>
{
    public readonly DefaultContext _context;
    public readonly ILogger<T> _logger;
    public BaseRepository(DefaultContext context, ILogger<T> logger)
    {
        _context = context;
        _logger = logger;
    }
}
