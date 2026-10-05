using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechStore.Domain.Entities;

namespace TechStore.Infrastructure.Mappings;

/// <summary>
/// Mapeamento Fluent API da entidade Produto.
/// Define constraints, tipos de coluna e índices.
/// </summary>
public class MapeadorDeProduto : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(250)
            .HasColumnType("nvarchar(250)");

        builder.Property(p => p.Descricao)
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)");

        builder.Property(p => p.Categoria)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar(100)");

        builder.Property(p => p.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.QuantidadeEstoque)
            .IsRequired();

        builder.Property(p => p.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.DataAtualizacao);

        builder.Property(p => p.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(p => p.Categoria)
            .HasDatabaseName("IX_Produtos_Categoria");

        builder.HasIndex(p => p.Ativo)
            .HasDatabaseName("IX_Produtos_Ativo");
    }
}
