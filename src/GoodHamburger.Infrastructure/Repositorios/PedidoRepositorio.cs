using System.Text.Json;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Infrastructure.Repositorios;

/// <summary>
/// Implementação do repositório de pedidos utilizando persistência em arquivo JSON.
/// </summary>
public class PedidoRepositorio : IPedidoRepositorio
{
    private readonly string _caminhoArquivo = "pedidos.json";
    private readonly List<Pedido> _pedidos = new();
    private readonly JsonSerializerOptions _opcoes = new() { WriteIndented = true };

    public PedidoRepositorio()
    {
        CarregarDadosIniciais();
    }

    private void CarregarDadosIniciais()
    {
        if (File.Exists(_caminhoArquivo))
        {
            var json = File.ReadAllText(_caminhoArquivo);
            var dados = JsonSerializer.Deserialize<List<Pedido>>(json);
            if (dados != null)
            {
                _pedidos.AddRange(dados);
            }
        }
    }

    private async Task SalvarAlteracoesAsync()
    {
        var json = JsonSerializer.Serialize(_pedidos, _opcoes);
        await File.WriteAllTextAsync(_caminhoArquivo, json);
    }

    public Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return Task.FromResult<IEnumerable<Pedido>>(_pedidos);
    }

    public Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        return Task.FromResult(_pedidos.FirstOrDefault(p => p.Id == id));
    }

    public async Task AdicionarAsync(Pedido pedido)
    {
        _pedidos.Add(pedido);
        await SalvarAlteracoesAsync();
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        var index = _pedidos.FindIndex(p => p.Id == pedido.Id);
        if (index != -1)
        {
            _pedidos[index] = pedido;
            await SalvarAlteracoesAsync();
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido != null)
        {
            _pedidos.Remove(pedido);
            await SalvarAlteracoesAsync();
        }
    }
}
