using Microsoft.AspNetCore.Mvc;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;

namespace TechStore.Api.Controllers;

/// <summary>
/// Controller REST para operações CRUD de Produtos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IServicoDeProduto _produtoService;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IServicoDeProduto produtoService, ILogger<ProdutosController> logger)
    {
        _produtoService = produtoService;
        _logger = logger;
    }

    /// <summary>
    /// Listar todos os produtos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoRespostaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsultaTodos()
    {
        _logger.LogInformation("GET /api/produtos — Listando todos os produtos.");

        var produtos = await _produtoService.ConsultaTodosAsync();

        return Ok(produtos);
    }

    /// <summary>
    /// Consultar produto por Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProdutoRespostaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConsultaPorId(int id)
    {
        _logger.LogInformation("GET /api/produtos/{ProdutoId} — Consultando produto.", id);

        var produto = await _produtoService.ConsultaPorIdAsync(id);

        if (produto is null)
        {
            return NotFound(new { message = $"Produto com Id {id} não encontrado." });
        }

        return Ok(produto);
    }

    /// <summary>
    /// Cadastrar novo produto
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoRespostaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cria([FromBody] CriaProdutoDto dto)
    {
        _logger.LogInformation("POST /api/produtos — Cadastrando produto: {ProdutoNome}", dto.Nome);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var produto = await _produtoService.CriaAsync(dto);

        return CreatedAtAction(
            nameof(Cria),
            new { id = produto.Id },
            produto);
    }

    /// <summary>
    /// Atualizar produto existente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProdutoRespostaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualiza(int id, [FromBody] AtualizaProdutoDto dto)
    {
        _logger.LogInformation("PUT /api/produtos/{ProdutoId} — Atualizando produto.", id);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var produto = await _produtoService.AtualizaAsync(id, dto);

        if (produto is null)
        {
            return NotFound(new { message = $"Produto com Id {id} não encontrado." });
        }

        return Ok(produto);
    }

    /// <summary>
    /// Remover produto (soft delete).
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        _logger.LogInformation("DELETE /api/produtos/{ProdutoId} — Removendo produto.", id);

        var result = await _produtoService.RemovaAsync(id);

        if (!result)
        {
            return NotFound(new { message = $"Produto com Id {id} não encontrado." });
        }

        return NoContent();
    }
}
