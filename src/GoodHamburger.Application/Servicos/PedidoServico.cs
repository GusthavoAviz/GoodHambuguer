using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Validadores;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Servicos;

/// <summary>
/// Implementação do serviço de pedidos.
/// </summary>
public class PedidoServico(IPedidoRepositorio repositorio, CriarPedidoValidador validador) : IPedidoServico
{
    /// <inheritdoc/>
    public async Task<PedidoResposta> CriarPedidoAsync(CriarPedidoRequisicao requisicao)
    {
        var resultadoValidacao = await validador.ValidateAsync(requisicao);
        if (!resultadoValidacao.IsValid)
        {
            throw new FluentValidation.ValidationException(resultadoValidacao.Errors);
        }

        var pedido = new Pedido
        {
            Itens = requisicao.Itens.Select(i => new Produto
            {
                Id = i.ProdutoId,
                Nome = i.Nome,
                Preco = i.Preco,
                Categoria = i.Categoria
            }).ToList()
        };

        await repositorio.AdicionarAsync(pedido);

        return MapearParaResposta(pedido);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PedidoResposta>> ObterTodosPedidosAsync()
    {
        var pedidos = await repositorio.ObterTodosAsync();
        return pedidos.Select(MapearParaResposta);
    }

    /// <inheritdoc/>
    public async Task<PedidoResposta?> ObterPedidoPorIdAsync(Guid id)
    {
        var pedido = await repositorio.ObterPorIdAsync(id);
        return pedido != null ? MapearParaResposta(pedido) : null;
    }

    /// <inheritdoc/>
    public async Task DeletarPedidoAsync(Guid id)
    {
        await repositorio.DeletarAsync(id);
    }

    /// <inheritdoc/>
    public async Task<decimal> ObterTotalVendasAsync()
    {
        var pedidos = await repositorio.ObterTodosAsync();
        return pedidos.Sum(p => p.CalcularResumo().Total);
    }

    /// <summary>
    /// Mapeia uma entidade de pedido para um DTO de resposta.
    /// </summary>
    private static PedidoResposta MapearParaResposta(Pedido pedido)
    {
        var resumo = pedido.CalcularResumo();
        return new PedidoResposta(
            pedido.Id,
            pedido.Itens.Select(i => new ItemPedidoResposta(i.Id, i.Nome, i.Preco, i.Categoria)).ToList(),
            resumo.Subtotal,
            resumo.Desconto,
            resumo.Total,
            pedido.CriadoEm
        );
    }
}
