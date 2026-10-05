using TechStore.Domain.Entities;

namespace TechStore.Domain.Interfaces;

/// <summary>
/// Contrato do repositório de Produtos.
/// </summary>
public interface IRepositorioDeProduto
{
    Task<IEnumerable<Produto>> ConsulteTodosAsync();

    Task<Produto?> ConsulteProdutoPorId(int id);

    Task<Produto> AdicionaAsync(Produto produto);

    Task AtualizaAsync(Produto produto);

    Task RemovaAsync(int id);
}
