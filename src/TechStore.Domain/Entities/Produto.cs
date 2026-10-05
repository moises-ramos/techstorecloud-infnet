namespace TechStore.Domain.Entities;

/// <summary>
/// Representa o Produto.
/// </summary>
public class Produto
{
    public int Id { get; set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do produto.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Categoria do produto (ex: Eletrônicos, Software, Acessórios).
    /// </summary>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Preço unitário do produto.
    /// </summary>
    public decimal Preco { get; set; }

    /// <summary>
    /// Quantidade disponível em estoque.
    /// </summary>
    public int QuantidadeEstoque { get; set; }

    /// <summary>
    /// Data de criação do registro.
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última atualização do registro.
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }

    /// <summary>
    /// Indica se o produto está ativo (remoção lógica).
    /// </summary>
    public bool Ativo { get; set; } = true;
}
