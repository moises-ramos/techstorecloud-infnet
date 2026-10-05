using Microsoft.Extensions.Logging;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Domain.Entities;
using TechStore.Domain.Interfaces;

namespace TechStore.Application.Services;

/// <summary>
/// Implementação da camada de serviço de Produtos.
/// </summary>
public class ServicoDeProduto : IServicoDeProduto
{
    private readonly IRepositorioDeProduto _repository;
    private readonly ILogger<ServicoDeProduto> _logger;

    public ServicoDeProduto(IRepositorioDeProduto repository, ILogger<ServicoDeProduto> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<ProdutoRespostaDto>> ConsultaTodosAsync()
    {
        _logger.LogInformation("Consultando todos os produtos ativos.");

        var produtos = await _repository.ConsulteTodosAsync();

        return produtos.Select(ConverteParaDto);
    }

    public async Task<ProdutoRespostaDto?> ConsultaPorIdAsync(int id)
    {
        _logger.LogInformation("Consultando produto com Id: {ProdutoId}", id);

        var produto = await _repository.ConsulteProdutoPorId(id);

        if (produto is null)
        {
            _logger.LogWarning("Produto com Id: {ProdutoId} não encontrado.", id);
            return null;
        }

        return ConverteParaDto(produto);
    }

    public async Task<ProdutoRespostaDto> CriaAsync(CriaProdutoDto dto)
    {
        _logger.LogInformation("Criando novo produto: {ProdutoNome}", dto.Nome);

        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            Preco = dto.Preco,
            QuantidadeEstoque = dto.QuantidadeEstoque,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var novoProduto = await _repository.AdicionaAsync(produto);

        _logger.LogInformation("Produto criado com sucesso. Id: {ProdutoId}, Nome: {ProdutoNome}",
            novoProduto.Id, novoProduto.Nome);

        return ConverteParaDto(novoProduto);
    }

    public async Task<ProdutoRespostaDto?> AtualizaAsync(int id, AtualizaProdutoDto dto)
    {
        _logger.LogInformation("Atualizando produto com Id: {ProdutoId}", id);

        var produto = await _repository.ConsulteProdutoPorId(id);

        if (produto is null)
        {
            _logger.LogWarning("Produto com Id: {ProdutoId} não encontrado para atualização.", id);
            return null;
        }

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Categoria = dto.Categoria;
        produto.Preco = dto.Preco;
        produto.QuantidadeEstoque = dto.QuantidadeEstoque;
        produto.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizaAsync(produto);

        _logger.LogInformation("Produto atualizado com sucesso. Id: {ProdutoId}", id);

        return ConverteParaDto(produto);
    }

    public async Task<bool> RemovaAsync(int id)
    {
        _logger.LogInformation("Removendo produto com Id: {ProdutoId}", id);

        var produto = await _repository.ConsulteProdutoPorId(id);

        if (produto is null)
        {
            _logger.LogWarning("Produto com Id: {ProdutoId} não encontrado para remoção.", id);
            return false;
        }

        await _repository.RemovaAsync(id);

        _logger.LogInformation("Produto removido com sucesso. Id: {ProdutoId}", id);

        return true;
    }

    private static ProdutoRespostaDto ConverteParaDto(Produto produto)
    {
        return new ProdutoRespostaDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Categoria = produto.Categoria,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            DataCriacao = produto.DataCriacao,
            DataAtualizacao = produto.DataAtualizacao,
            Ativo = produto.Ativo
        };
    }
}
