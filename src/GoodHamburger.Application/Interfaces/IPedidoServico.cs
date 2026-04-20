using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

/// <summary>
/// Contrato para o serviço de pedidos.
/// </summary>
public interface IPedidoServico
{
    /// <summary>
    /// Cria um novo pedido com base na requisição fornecida.
    /// </summary>
    Task<PedidoResposta> CriarPedidoAsync(CriarPedidoRequisicao requisicao);

    /// <summary>
    /// Obtém todos os pedidos registrados.
    /// </summary>
    Task<IEnumerable<PedidoResposta>> ObterTodosPedidosAsync();

    /// <summary>
    /// Obtém um pedido específico pelo seu ID.
    /// </summary>
    Task<PedidoResposta?> ObterPedidoPorIdAsync(Guid id);

    /// <summary>
    /// Remove um pedido do sistema.
    /// </summary>
    Task DeletarPedidoAsync(Guid id);

    /// <summary>
    /// Obtém o valor total de todas as vendas realizadas.
    /// </summary>
    Task<decimal> ObterTotalVendasAsync();
}
