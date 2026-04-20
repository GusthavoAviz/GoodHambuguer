using GoodHamburger.Domain.Entidades;

namespace GoodHamburger.Domain.Interfaces;

/// <summary>
/// Contrato para o repositório de pedidos.
/// </summary>
public interface IPedidoRepositorio
{
    /// <summary>
    /// Obtém todos os pedidos registrados.
    /// </summary>
    Task<IEnumerable<Pedido>> ObterTodosAsync();

    /// <summary>
    /// Obtém um pedido pelo seu identificador único.
    /// </summary>
    Task<Pedido?> ObterPorIdAsync(Guid id);

    /// <summary>
    /// Adiciona um novo pedido ao sistema.
    /// </summary>
    Task AdicionarAsync(Pedido pedido);

    /// <summary>
    /// Atualiza um pedido existente.
    /// </summary>
    Task AtualizarAsync(Pedido pedido);

    /// <summary>
    /// Remove um pedido pelo seu identificador.
    /// </summary>
    Task DeletarAsync(Guid id);
}
