using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entidades;

/// <summary>
/// Representa um produto no sistema.
/// </summary>
public class Produto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public CategoriaProduto Categoria { get; set; }
}
