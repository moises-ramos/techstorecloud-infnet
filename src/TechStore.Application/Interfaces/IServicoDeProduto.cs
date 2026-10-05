using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces;

/// <summary>
/// Contrato da camada de serviço de Produtos.
/// Define as operações de negócio disponíveis.
/// </summary>
public interface IServicoDeProduto
{
    /// <summary>
    /// Retorna todos os produtos ativos.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ProdutoRespostaDto>> ConsultaTodosAsync();

    /// <summary>
    /// Retorna um produto pelo Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProdutoRespostaDto?> ConsultaPorIdAsync(int id);

    /// <summary>
    /// Cria um novo produto.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ProdutoRespostaDto> CriaAsync(CriaProdutoDto dto);

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<ProdutoRespostaDto?> AtualizaAsync(int id, AtualizaProdutoDto dto);

    /// <summary>
    /// Remove (logicamente) um produto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> RemovaAsync(int id);
}
