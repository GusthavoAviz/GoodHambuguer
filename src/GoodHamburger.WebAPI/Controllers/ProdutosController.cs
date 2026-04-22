using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController(IProdutoServico produtoServico) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResposta>>> ObterTodos()
    {
        var produtos = await produtoServico.ObterTodosAsync();
        return Ok(produtos);
    }

    [HttpGet("ativos")]
    public async Task<ActionResult<IEnumerable<ProdutoResposta>>> ObterAtivos()
    {
        var produtos = await produtoServico.ObterAtivosAsync();
        return Ok(produtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResposta>> Criar([FromBody] CriarProdutoRequisicao requisicao)
    {
        var produto = await produtoServico.CriarAsync(requisicao);
        return CreatedAtAction(nameof(ObterTodos), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarProdutoRequisicao requisicao)
    {
        await produtoServico.AtualizarAsync(id, requisicao);
        return NoContent();
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> AlternarStatus(Guid id)
    {
        await produtoServico.AlternarStatusAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await produtoServico.DeletarAsync(id);
        return NoContent();
    }
}
