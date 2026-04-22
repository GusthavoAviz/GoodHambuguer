using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GoodHamburger.Tests;

public class ProdutoIntegracaoTestes : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProdutoIntegracaoTestes(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CriarProduto_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var requisicao = new CriarProdutoRequisicao("Novo Sanduba", 12.50m, CategoriaProduto.Sanduiche);

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/produtos", requisicao);

        // Assert
        resposta.StatusCode.Should().Be(HttpStatusCode.Created);
        var produto = await resposta.Content.ReadFromJsonAsync<ProdutoResposta>();
        produto!.Nome.Should().Be("Novo Sanduba");
        produto.Preco.Should().Be(12.50m);
    }

    [Theory]
    [InlineData("", 10.0)]
    [InlineData("   ", 10.0)]
    [InlineData("Produto Caro", 0)]
    [InlineData("Produto Grátis", -1.0)]
    public async Task CriarProduto_ComDadosInvalidos_NaoDevePermitir(string nome, decimal preco)
    {
        // Nota: A validação de nome/preço foi implementada no Frontend e no Serviço. 
        // Vamos validar o comportamento da API.
        var requisicao = new CriarProdutoRequisicao(nome, preco, CategoriaProduto.Sanduiche);

        // Act
        var resposta = await _client.PostAsJsonAsync("/api/produtos", requisicao);

        // Assert
        // Como o Serviço e o Controller podem lançar exceções ou retornar erros, 
        // dependendo da implementação exata, esperamos que não seja sucesso se validado.
        if (string.IsNullOrWhiteSpace(nome) || preco <= 0)
        {
             // Atualmente o controller não tem FluentValidation para Produtos, mas o serviço deveria barrar.
             // Se o sistema for robusto, deve retornar erro.
             resposta.IsSuccessStatusCode.Should().BeFalse();
        }
    }

    [Fact]
    public async Task AtualizarProduto_DeveAlterarDadosCorretamente()
    {
        // Arrange - Criar primeiro
        var novo = new CriarProdutoRequisicao("Original", 10.0m, CategoriaProduto.Bebida);
        var resPost = await _client.PostAsJsonAsync("/api/produtos", novo);
        var produtoCriado = await resPost.Content.ReadFromJsonAsync<ProdutoResposta>();

        // Act - Atualizar
        var atualizacao = new CriarProdutoRequisicao("Atualizado", 15.0m, CategoriaProduto.Sanduiche);
        var resPut = await _client.PutAsJsonAsync($"/api/produtos/{produtoCriado!.Id}", atualizacao);

        // Assert
        resPut.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var resGet = await _client.GetAsync("/api/produtos");
        var produtos = await resGet.Content.ReadFromJsonAsync<IEnumerable<ProdutoResposta>>();
        produtos.Should().Contain(p => p.Id == produtoCriado.Id && p.Nome == "Atualizado" && p.Preco == 15.0m);
    }

    [Fact]
    public async Task AlternarStatus_DeveMudarDisponibilidadeNoCardapio()
    {
        // Arrange
        var novo = new CriarProdutoRequisicao("Produto Volátil", 5.0m, CategoriaProduto.Acompanhamento);
        var resPost = await _client.PostAsJsonAsync("/api/produtos", novo);
        var produto = await resPost.Content.ReadFromJsonAsync<ProdutoResposta>();

        // Act - Desativar
        await _client.PutAsync($"/api/produtos/{produto!.Id}/status", null);

        // Assert - Não deve aparecer no menu de ativos
        var resMenu = await _client.GetAsync("/api/pedidos/menu");
        var menu = await resMenu.Content.ReadFromJsonAsync<IEnumerable<ProdutoResposta>>();
        menu.Should().NotContain(p => p.Id == produto.Id);

        // Act - Reativar
        await _client.PutAsync($"/api/produtos/{produto!.Id}/status", null);

        // Assert - Deve voltar ao menu
        resMenu = await _client.GetAsync("/api/pedidos/menu");
        menu = await resMenu.Content.ReadFromJsonAsync<IEnumerable<ProdutoResposta>>();
        menu.Should().Contain(p => p.Id == produto.Id);
    }

    [Fact]
    public async Task DeletarProduto_DeveRemoverDoSistema()
    {
        // Arrange
        var novo = new CriarProdutoRequisicao("Produto Efêmero", 1.0m, CategoriaProduto.Bebida);
        var resPost = await _client.PostAsJsonAsync("/api/produtos", novo);
        var produto = await resPost.Content.ReadFromJsonAsync<ProdutoResposta>();

        // Act
        var resDel = await _client.DeleteAsync($"/api/produtos/{produto!.Id}");

        // Assert
        resDel.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var resGet = await _client.GetAsync("/api/produtos");
        var produtos = await resGet.Content.ReadFromJsonAsync<IEnumerable<ProdutoResposta>>();
        produtos.Should().NotContain(p => p.Id == produto.Id);
    }
}
