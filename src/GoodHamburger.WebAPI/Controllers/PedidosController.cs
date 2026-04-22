using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.WebAPI.Controllers;

/// <summary>
/// Controller para gerenciamento de pedidos e menu.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PedidosController(IPedidoServico pedidoServico, IProdutoServico produtoServico) : ControllerBase
{
    /// <summary>
    /// Lista o cardápio disponível (produtos ativos) no Good Hamburger.
    /// </summary>
    [HttpGet("menu")]
    public async Task<IActionResult> ObterMenu()
    {
        var menu = await produtoServico.ObterAtivosAsync();
        return Ok(menu);
    }

    /// <summary>
    /// Registra um novo pedido no sistema.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PedidoResposta>> CriarPedido([FromBody] CriarPedidoRequisicao requisicao)
    {
        try
        {
            var pedido = await pedidoServico.CriarPedidoAsync(requisicao);
            return CreatedAtAction(nameof(ObterPedidoPorId), new { id = pedido.Id }, pedido);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new { mensagem = "Erro de validação", detalhes = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro interno no servidor", detalhes = ex.Message });
        }
    }

    /// <summary>
    /// Obtém o histórico de todos os pedidos realizados.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResposta>>> ObterTodos()
    {
        var pedidos = await pedidoServico.ObterTodosPedidosAsync();
        return Ok(pedidos);
    }

    /// <summary>
    /// Busca um pedido específico pelo seu identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PedidoResposta>> ObterPedidoPorId(Guid id)
    {
        var pedido = await pedidoServico.ObterPedidoPorIdAsync(id);
        if (pedido == null) return NotFound(new { mensagem = "Pedido não encontrado" });

        return Ok(pedido);
    }

    /// <summary>
    /// Remove um pedido do sistema.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletarPedido(Guid id)
    {
        var pedido = await pedidoServico.ObterPedidoPorIdAsync(id);
        if (pedido == null) return NotFound(new { mensagem = "Pedido não encontrado" });

        await pedidoServico.DeletarPedidoAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Obtém estatísticas de vendas, como o valor total vendido.
    /// </summary>
    [HttpGet("estatisticas")]
    public async Task<IActionResult> ObterEstatisticas()
    {
        var totalVendas = await pedidoServico.ObterTotalVendasAsync();
        return Ok(new { totalVendido = totalVendas });
    }
}
