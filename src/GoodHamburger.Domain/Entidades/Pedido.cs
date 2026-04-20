using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.ObjetosValor;

namespace GoodHamburger.Domain.Entidades;

/// <summary>
/// Representa um pedido no sistema Good Hamburger.
/// </summary>
public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Produto> Itens { get; set; } = new();
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Calcula o resumo financeiro do pedido com base nas regras de desconto.
    /// </summary>
    /// <returns>Resumo contendo subtotal, desconto e total final.</returns>
    public ResumoPedido CalcularResumo()
    {
        var subtotal = Itens.Sum(x => x.Preco);
        var percentualDesconto = 0m;

        var temSanduiche = Itens.Any(x => x.Categoria == CategoriaProduto.Sanduiche);
        var temAcompanhamento = Itens.Any(x => x.Categoria == CategoriaProduto.Acompanhamento);
        var temBebida = Itens.Any(x => x.Categoria == CategoriaProduto.Bebida);

        if (temSanduiche && temAcompanhamento && temBebida)
        {
            percentualDesconto = 0.20m;
        }
        else if (temSanduiche && temBebida)
        {
            percentualDesconto = 0.15m;
        }
        else if (temSanduiche && temAcompanhamento)
        {
            percentualDesconto = 0.10m;
        }

        var desconto = subtotal * percentualDesconto;
        var total = subtotal - desconto;

        return new ResumoPedido(subtotal, desconto, total);
    }
}
