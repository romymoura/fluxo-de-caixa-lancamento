using FluxoDeCaixa.Lancamento.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluxoDeCaixa.Lancamento.ORM.Mapping;

public class ProductConfigurationMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        // Chave primária
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        // Propriedades
        builder.Property(x => x.MessageId).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Subtotal).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Description).IsRequired().HasMaxLength(100);

        // Configura a propriedade StoreId como chave estrangeira
        builder.HasOne(p => p.Store) // Propriedade de navegação
              .WithMany(s => s.Products) // Coleção na entidade Store
              .HasForeignKey(p => p.StoreId) // Propriedade que é a FK
              .OnDelete(DeleteBehavior.Restrict); // Opcional: define o comportamento de exclusão

        builder.Property(x => x.StoreId).IsRequired();
    }
}