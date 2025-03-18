namespace FluxoDeCaixa.Lancamento.Domain.Entities;
public class Store : BaseEntity
{
    public Store()
    {
        CreatedAt = DateTime.UtcNow;
    }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; }
}