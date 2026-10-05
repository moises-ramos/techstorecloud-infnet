using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs;

/// <summary>
/// DTO para criação de um novo produto.
/// </summary>
public class CriaProdutoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 200 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    [StringLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres.")]
    public string Categoria { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 999999.99, ErrorMessage = "O preço deve ser entre R$ 0,01 e R$ 999.999,99.")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser um valor positivo.")]
    public int QuantidadeEstoque { get; set; }
}
