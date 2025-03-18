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
        builder.Property(x => x.IdStore).IsRequired().HasMaxLength(36);


        // Configura o relacionamento com a entidade Store
        builder.HasOne(p => p.Store)              // Um produto pertence a uma loja
               .WithMany(s => s.Products)         // Uma loja tem muitos produtos
               .HasForeignKey(p => p.IdStore)    // Chave estrangeira
               .OnDelete(DeleteBehavior.Cascade); // Comportamento ao deletar (opcional)


        builder.Property(x => x.IdMessage).IsRequired().HasMaxLength(36);
        builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Amount).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
    }
}