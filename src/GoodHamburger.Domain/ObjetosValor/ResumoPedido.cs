namespace GoodHamburger.Domain.ObjetosValor;

/// <summary>
/// Representa o resumo financeiro de um pedido.
/// </summary>
public record ResumoPedido(decimal Subtotal, decimal Desconto, decimal Total);
