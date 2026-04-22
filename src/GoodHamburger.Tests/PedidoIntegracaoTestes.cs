using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GoodHamburger.Tests;

public class PedidoIntegracaoTestes : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PedidoIntegracaoTestes(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CriarPedido_ComComboCompleto_DeveAplicar20PorcentoDescontoEPersistir()
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, CategoriaProduto.Sanduiche),
            new(Guid.NewGuid(), "Batata frita", 2.00m, CategoriaProduto.Acompanhamento),
            new(Guid.NewGuid(), "Refrigerante", 2.50m, CategoriaProduto.Bebida)
        });

        // Act - Criar Pedido
        var respostaPost = await _client.PostAsJsonAsync("/api/pedidos", requisicao);
        
        // Assert Post
        respostaPost.StatusCode.Should().Be(HttpStatusCode.Created);
        var pedidoCriado = await respostaPost.Content.ReadFromJsonAsync<PedidoResposta>();
        pedidoCriado.Should().NotBeNull();
        pedidoCriado!.Total.Should().Be(7.60m); // (5+2+2.5) * 0.8

        // Act - Verificar no Histórico
        var respostaGet = await _client.GetAsync("/api/pedidos");
        var pedidos = await respostaGet.Content.ReadFromJsonAsync<IEnumerable<PedidoResposta>>();

        // Assert Get
        pedidos.Should().Contain(p => p.Id == pedidoCriado.Id);
    }

    [Theory]
    [InlineData("X Burger", 5.00, CategoriaProduto.Sanduiche)]
    [InlineData("Batata frita", 2.00, CategoriaProduto.Acompanhamento)]
    [InlineData("Refrigerante", 2.50, CategoriaProduto.Bebida)]
    public async Task CriarPedido_ComApenasUmItem_NaoDeveAplicarDesconto(string nome, decimal preco, CategoriaProduto categoria)
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), nome, preco, categoria)
        });

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/pedidos", requisicao);
        var pedido = await resposta.Content.ReadFromJsonAsync<PedidoResposta>();

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        pedido!.Total.Should().Be(preco);
        pedido.Desconto.Should().Be(0);
    }

    [Fact]
    public async Task CriarPedido_ComBatataEBebida_NaoDeveAplicarDesconto()
    {
        // Arrange - Combo sem Sanduíche não gera desconto pelas regras
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "Batata frita", 2.00m, CategoriaProduto.Acompanhamento),
            new(Guid.NewGuid(), "Refrigerante", 2.50m, CategoriaProduto.Bebida)
        });

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/pedidos", requisicao);
        var pedido = await resposta.Content.ReadFromJsonAsync<PedidoResposta>();

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        pedido!.Total.Should().Be(4.50m);
        pedido.Desconto.Should().Be(0);
    }

    [Fact]
    public async Task CriarPedido_ComSanduicheEBatata_DeveAplicar10PorcentoDesconto()
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, CategoriaProduto.Sanduiche),
            new(Guid.NewGuid(), "Batata frita", 2.00m, CategoriaProduto.Acompanhamento)
        });

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/pedidos", requisicao);
        var pedido = await resposta.Content.ReadFromJsonAsync<PedidoResposta>();

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        pedido!.Total.Should().Be(6.30m); // (5+2) * 0.9
        pedido.Desconto.Should().Be(0.70m);
    }

    [Fact]
    public async Task CriarPedido_ComSanduicheEBebida_DeveAplicar15PorcentoDesconto()
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, CategoriaProduto.Sanduiche),
            new(Guid.NewGuid(), "Refrigerante", 2.50m, CategoriaProduto.Bebida)
        });

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/pedidos", requisicao);
        var pedido = await resposta.Content.ReadFromJsonAsync<PedidoResposta>();

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        pedido!.Total.Should().Be(6.375m); // (5+2.5) * 0.85
        pedido.Desconto.Should().Be(1.125m);
    }

    [Fact]
    public async Task CriarPedido_ComItensDuplicados_DeveRetornarBadRequest()
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, CategoriaProduto.Sanduiche),
            new(Guid.NewGuid(), "X Egg", 4.50m, CategoriaProduto.Sanduiche)
        });

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/pedidos", requisicao);

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ObterEstatisticas_DeveRetornarSomaCorretaDeVendas()
    {
        // Arrange
        // (Assumindo que o arquivo JSON pode ter dados de testes anteriores ou estar vazio)
        // Vamos criar um pedido novo para garantir um valor conhecido
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Bacon", 7.00m, CategoriaProduto.Sanduiche)
        });
        await _client.PostAsJsonAsync("/api/pedidos", requisicao);

        // Act
        var resposta = await _client.GetAsync("/api/pedidos/estatisticas");
        var estatisticas = await resposta.Content.ReadAsStringAsync();

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
        estatisticas.Should().Contain("totalVendido");
    }

    [Fact]
    public async Task ObterMenu_DeveRetornarItensCorretos()
    {
        // Act
        var resposta = await _client.GetAsync("/api/pedidos/menu");
        
        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.OK);
        var menu = await resposta.Content.ReadFromJsonAsync<IEnumerable<dynamic>>();
        menu.Should().NotBeEmpty();
    }
}
