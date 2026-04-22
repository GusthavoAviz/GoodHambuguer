using GoodHamburger.Domain.Entidades;

namespace GoodHamburger.Domain.Interfaces;

/// <summary>
/// Contrato para o repositório de produtos (cardápio).
/// </summary>
public interface IProdutoRepositorio
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task DeletarAsync(Guid id);
}
