using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Servicos;

public class ProdutoServico(IProdutoRepositorio repositorio) : IProdutoServico
{
    public async Task<IEnumerable<ProdutoResposta>> ObterTodosAsync()
    {
        var produtos = await repositorio.ObterTodosAsync();
        return produtos.Select(MapearParaResposta);
    }

    public async Task<IEnumerable<ProdutoResposta>> ObterAtivosAsync()
    {
        var produtos = await repositorio.ObterTodosAsync();
        return produtos.Where(p => p.Ativo).Select(MapearParaResposta);
    }

    public async Task<ProdutoResposta> CriarAsync(CriarProdutoRequisicao requisicao)
    {
        ValidarProduto(requisicao);

        var produto = new Produto
        {
            Nome = requisicao.Nome.Trim(),
            Preco = requisicao.Preco,
            Categoria = requisicao.Categoria,
            Ativo = true
        };

        await repositorio.AdicionarAsync(produto);
        return MapearParaResposta(produto);
    }

    public async Task AtualizarAsync(Guid id, CriarProdutoRequisicao requisicao)
    {
        ValidarProduto(requisicao);

        var produto = await repositorio.ObterPorIdAsync(id);
        if (produto != null)
        {
            produto.Nome = requisicao.Nome.Trim();
            produto.Preco = requisicao.Preco;
            produto.Categoria = requisicao.Categoria;
            await repositorio.AtualizarAsync(produto);
        }
    }

    private void ValidarProduto(CriarProdutoRequisicao r)
    {
        if (string.IsNullOrWhiteSpace(r.Nome))
            throw new ArgumentException("O nome do produto não pode estar vazio.");
        
        if (r.Preco <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");
    }

    public async Task AlternarStatusAsync(Guid id)
    {
        var produto = await repositorio.ObterPorIdAsync(id);
        if (produto != null)
        {
            produto.Ativo = !produto.Ativo;
            await repositorio.AtualizarAsync(produto);
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        await repositorio.DeletarAsync(id);
    }

    private static ProdutoResposta MapearParaResposta(Produto p) =>
        new(p.Id, p.Nome, p.Preco, p.Categoria, p.Ativo);
}
