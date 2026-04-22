using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IProdutoServico
{
    Task<IEnumerable<ProdutoResposta>> ObterTodosAsync();
    Task<IEnumerable<ProdutoResposta>> ObterAtivosAsync();
    Task<ProdutoResposta> CriarAsync(CriarProdutoRequisicao requisicao);
    Task AtualizarAsync(Guid id, CriarProdutoRequisicao requisicao);
    Task AlternarStatusAsync(Guid id);
    Task DeletarAsync(Guid id);
}
