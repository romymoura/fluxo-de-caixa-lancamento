using AutoMapper;
using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.Application.Services;
public class BaseService<T> where T : class
{
    public readonly IMapper _mapper;
    public readonly ILogger<T> _logger;
    public BaseService(IMapper mapper, ILogger<T> logger)
    {
        _mapper = mapper;
        _logger = logger;
    }
}
