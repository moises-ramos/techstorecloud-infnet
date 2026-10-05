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
        // Tabela
        builder.ToTable("Produtos");

        // Chave primária
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        // Nome - obrigatório, máximo 200 caracteres
        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        // Descrição - opcional, máximo 1000 caracteres
        builder.Property(p => p.Descricao)
            .HasMaxLength(1000)
            .HasColumnType("nvarchar(1000)");

        // Categoria - obrigatória, máximo 100 caracteres
        builder.Property(p => p.Categoria)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar(100)");

        // Preço - decimal(18,2) para valores monetários
        builder.Property(p => p.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        // Quantidade em Estoque
        builder.Property(p => p.QuantidadeEstoque)
            .IsRequired();

        // Data de Criação - valor padrão no banco
        builder.Property(p => p.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Data de Atualização - nullable
        builder.Property(p => p.DataAtualizacao);

        // Ativo - default true
        builder.Property(p => p.Ativo)
            .IsRequired()
            .HasDefaultValue(true);

        // Índice para consultas por categoria
        builder.HasIndex(p => p.Categoria)
            .HasDatabaseName("IX_Produtos_Categoria");

        // Índice para filtrar ativos
        builder.HasIndex(p => p.Ativo)
            .HasDatabaseName("IX_Produtos_Ativo");
    }
}
