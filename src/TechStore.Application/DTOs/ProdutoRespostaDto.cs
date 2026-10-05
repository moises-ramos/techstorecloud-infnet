namespace TechStore.Application.DTOs;

public class ProdutoRespostaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    public int QuantidadeEstoque { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime? DataAtualizacao { get; set; }

    public bool Ativo { get; set; }
}
