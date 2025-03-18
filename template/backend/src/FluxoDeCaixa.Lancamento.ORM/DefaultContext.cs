using FluxoDeCaixa.Lancamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FluxoDeCaixa.Lancamento.ORM;
public class DefaultContext : DbContext
{
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}