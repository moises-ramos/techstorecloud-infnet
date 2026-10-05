using TechStore.Domain.Entities;

namespace TechStore.Domain.Interfaces;

/// <summary>
/// Contrato do repositório de Produtos.
/// </summary>
public interface IRepositorioDeProduto
{
    /// <summary>
    /// Consulta todos os produtos que estão ativos.
    /// </summary>
    /// <returns>retorna todos os produtos cadastrados.</returns>
    Task<IEnumerable<Produto>> ConsulteTodosAsync();

    /// <summary>
    /// Consulta produto por ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>retorna o produto encontrado.</returns>
    Task<Produto?> ConsulteProdutoPorId(int id);

    /// <summary>
    /// Insere o produto.
    /// </summary>
    /// <param name="produto"></param>
    /// <returns></returns>
    Task<Produto> AdicionaAsync(Produto produto);

    /// <summary>
    /// Atualize os dados do produto.
    /// </summary>
    /// <param name="produto"></param>
    /// <returns></returns>
    Task AtualizaAsync(Produto produto);

    /// <summary>
    /// Remove o produto de forma lógica.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RemovaAsync(int id);
}
