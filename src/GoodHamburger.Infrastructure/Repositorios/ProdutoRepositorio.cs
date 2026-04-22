using System.Text.Json;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Infrastructure.Repositorios;

public class ProdutoRepositorio : IProdutoRepositorio
{
    private readonly string _caminhoArquivo = "produtos.json";
    private readonly List<Produto> _produtos = new();
    private readonly JsonSerializerOptions _opcoes = new() { WriteIndented = true };

    public ProdutoRepositorio()
    {
        CarregarDadosIniciais();
    }

    private void CarregarDadosIniciais()
    {
        if (File.Exists(_caminhoArquivo))
        {
            var json = File.ReadAllText(_caminhoArquivo);
            var dados = JsonSerializer.Deserialize<List<Produto>>(json);
            if (dados != null) _produtos.AddRange(dados);
        }
        else
        {
            // Carga inicial padrão
            _produtos.AddRange(new List<Produto>
            {
                new() { Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaProduto.Sanduiche },
                new() { Nome = "X Egg", Preco = 4.50m, Categoria = CategoriaProduto.Sanduiche },
                new() { Nome = "X Bacon", Preco = 7.00m, Categoria = CategoriaProduto.Sanduiche },
                new() { Nome = "Batata frita", Preco = 2.00m, Categoria = CategoriaProduto.Acompanhamento },
                new() { Nome = "Refrigerante", Preco = 2.50m, Categoria = CategoriaProduto.Bebida }
            });
            SalvarAlteracoesAsync().Wait();
        }
    }

    private async Task SalvarAlteracoesAsync()
    {
        var json = JsonSerializer.Serialize(_produtos, _opcoes);
        await File.WriteAllTextAsync(_caminhoArquivo, json);
    }

    public Task<IEnumerable<Produto>> ObterTodosAsync() => Task.FromResult<IEnumerable<Produto>>(_produtos);

    public Task<Produto?> ObterPorIdAsync(Guid id) => Task.FromResult(_produtos.FirstOrDefault(p => p.Id == id));

    public async Task AdicionarAsync(Produto produto)
    {
        _produtos.Add(produto);
        await SalvarAlteracoesAsync();
    }

    public async Task AtualizarAsync(Produto produto)
    {
        var index = _produtos.FindIndex(p => p.Id == produto.Id);
        if (index != -1)
        {
            _produtos[index] = produto;
            await SalvarAlteracoesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto != null)
        {
            _produtos.Remove(produto);
            await SalvarAlteracoesAsync();
        }
    }
}
