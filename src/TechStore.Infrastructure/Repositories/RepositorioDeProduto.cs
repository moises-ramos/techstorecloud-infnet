using Microsoft.EntityFrameworkCore;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;
using TechStore.Infrastructure.Data;

namespace TechStore.Infrastructure.Repositories;

public class RepositorioDeProduto : IRepositorioDeProduto
{
    private readonly AppDbContext _context;

    public RepositorioDeProduto(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ConsulteTodosAsync()
    {
        return await _context.Produtos
            .Where(p => p.Ativo)
            .OrderByDescending(p => p.DataCriacao)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Produto?> ConsulteProdutoPorId(int id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);
    }

    public async Task<Produto> AdicionaAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task AtualizaAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        await _context.SaveChangesAsync();
    }

    public async Task RemovaAsync(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is not null)
        {
            produto.Ativo = false;
            produto.DataAtualizacao = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
