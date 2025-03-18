using FluxoDeCaixa.Lancamento.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FluxoDeCaixa.Lancamento.ORM.Repositories;

/// <summary>
/// Base dos repositórios
/// </summary>
/// <typeparam name="TILogger">A classe que está processando o metodo</typeparam>
/// <typeparam name="TEntity">Entidade do banco</typeparam>
/// <typeparam name="TKey">Tipo de chave, (Guid, Int ou Etc)</typeparam>
public class BaseRepository<TILogger, TEntity> : IBaseRepository<TEntity> 
    where TEntity : class, new() 
{
    public readonly DefaultContext _context;
    public readonly ILogger<TILogger> _logger;
    public BaseRepository(DefaultContext context, ILogger<TILogger> logger)
    {
        _context = context;
        _logger = logger;
    }
    public virtual async Task<TEntity> CreateAsync<TKey>(TEntity item, CancellationToken cancellationToken = default)
    {
        var name = typeof(TEntity).Name;
        var id = GetId<TKey>(item);
        _logger.LogInformation($"Iniciando criação de {name}");
        await _context.Set<TEntity>().AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Concluída criação de {name} com ID: {id}");
        return item;
    }
    public virtual async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var name = typeof(TEntity).Name;
        _logger.LogInformation($"Iniciando exclusão de {name} com ID: {id}");
        var entity = await _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id, cancellationToken);

        if (entity == null)
        {
            _logger.LogWarning($"{name} com ID: {id} não encontrado para exclusão.");
            return false;
        }
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Concluída exclusão de {name} com ID: {id}");
        return true;
    }
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var name = typeof(TEntity).Name;
        _logger.LogInformation($"Inicio resgatando dados da {name}");
        var item = await _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == id, cancellationToken);
        _logger.LogInformation($"Fim resgatando dados da {name}");
        return item;
    }
    public virtual async Task<bool> UpdateAsync<TKey>(TEntity item, CancellationToken cancellationToken = default)
    {
        var name = typeof(TEntity).Name;
        var idItem = GetId<TKey>(item);
        _logger.LogInformation($"Iniciando atualização de {name} com ID: {idItem}");


        var existingEntity = await _context.Set<TEntity>()
            .FirstOrDefaultAsync(x => EF.Property<TKey>(x, "Id")!.Equals(idItem), cancellationToken);

        if (existingEntity == null)
        {
            _logger.LogWarning($"{name} com ID: {idItem} não encontrado para atualização.");
            return false;
        }

        // Atualiza a entidade existente com os valores da entidade fornecida
        _context.Entry(existingEntity).CurrentValues.SetValues(item);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Concluída atualização de {name} com ID: {idItem}");

        return true;
    }

    private TKey GetId<TKey>(TEntity entity)
    {
        var objEntity = typeof(TEntity);
        var name = objEntity.Name;

        // Encontra o nome da propriedade que representa a chave primária
        var keyName = _context.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties
            .Select(p => p.Name)
            .FirstOrDefault();

        if (keyName == null)
        {
            throw new InvalidOperationException($"Entidade {name} não possui uma chave primária definida.");
        }

        // Usa reflexão para obter o valor da propriedade
        var propertyInfo = objEntity.GetProperty(keyName);
        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"A propriedade {keyName} não foi encontrada na entidade {name}.");
        }

        var idValue = propertyInfo.GetValue(entity);
        if (idValue == null)
        {
            throw new InvalidOperationException($"O valor da chave primária da entidade {name} é nulo.");
        }

        return (TKey)idValue;
    }
}
