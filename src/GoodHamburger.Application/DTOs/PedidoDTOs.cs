using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

/// <summary>
/// Requisição para item de pedido.
/// </summary>
public record ItemPedidoRequisicao(Guid ProdutoId, string Nome, decimal Preco, CategoriaProduto Categoria);

/// <summary>
/// Requisição para criação de um novo pedido.
/// </summary>
public record CriarPedidoRequisicao(List<ItemPedidoRequisicao> Itens);

/// <summary>
/// Resposta de um item de pedido.
/// </summary>
public record ItemPedidoResposta(Guid ProdutoId, string Nome, decimal Preco, CategoriaProduto Categoria);

/// <summary>
/// Resposta detalhada de um pedido.
/// </summary>
public record PedidoResposta(
    Guid Id, 
    List<ItemPedidoResposta> Itens, 
    decimal Subtotal, 
    decimal Desconto, 
    decimal Total, 
    DateTime CriadoEm);

/// <summary>
/// Requisição para criar um novo produto no cardápio.
/// </summary>
public record CriarProdutoRequisicao(string Nome, decimal Preco, CategoriaProduto Categoria);

/// <summary>
/// Resposta com dados do produto.
/// </summary>
public record ProdutoResposta(Guid Id, string Nome, decimal Preco, CategoriaProduto Categoria, bool Ativo);
