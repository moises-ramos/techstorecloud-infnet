using TechStore.Application.DTOs;

namespace TechStore.Application.Interfaces;

public interface IServicoDeProduto
{
    Task<IEnumerable<ProdutoRespostaDto>> ConsultaTodosAsync();

    Task<ProdutoRespostaDto?> ConsultaPorIdAsync(int id);

    Task<ProdutoRespostaDto> CriaAsync(CriaProdutoDto dto);

    Task<ProdutoRespostaDto?> AtualizaAsync(int id, AtualizaProdutoDto dto);

    Task<bool> RemovaAsync(int id);
}
