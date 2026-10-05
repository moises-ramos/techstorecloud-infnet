using Microsoft.EntityFrameworkCore;
using TechStore.Domain.Entities;
using TechStore.Infrastructure.Mappings;

namespace TechStore.Infrastructure.Data;

/// <summary>
/// Contexto do Entity Framework Core para acesso ao Azure SQL Database.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>Tabela de Produtos.</summary>
    public DbSet<Produto> Produtos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica o mapeamento Fluent API do Produto
        modelBuilder.ApplyConfiguration(new MapeadorDeProduto());
    }
}
