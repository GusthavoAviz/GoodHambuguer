using FluentAssertions;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using Xunit;

namespace GoodHamburger.Tests;

public class PedidoTestes
{
    [Fact]
    public void CalcularResumo_ComSanduicheBatataEBebida_DeveAplicar20PorcentoDesconto()
    {
        // Arrange
        var pedido = new Pedido();
        pedido.Itens.Add(new Produto { Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaProduto.Sanduiche });
        pedido.Itens.Add(new Produto { Nome = "Batata frita", Preco = 2.00m, Categoria = CategoriaProduto.Acompanhamento });
        pedido.Itens.Add(new Produto { Nome = "Refrigerante", Preco = 2.50m, Categoria = CategoriaProduto.Bebida });

        // Act
        var resumo = pedido.CalcularResumo();

        // Assert
        resumo.Subtotal.Should().Be(9.50m);
        resumo.Desconto.Should().Be(1.90m);
        resumo.Total.Should().Be(7.60m);
    }

    [Fact]
    public void CalcularResumo_ApenasSanduiche_NaoDeveAplicarDesconto()
    {
        // Arrange
        var pedido = new Pedido();
        pedido.Itens.Add(new Produto { Nome = "X Burger", Preco = 5.00m, Categoria = CategoriaProduto.Sanduiche });

        // Act
        var resumo = pedido.CalcularResumo();

        // Assert
        resumo.Subtotal.Should().Be(5.00m);
        resumo.Desconto.Should().Be(0m);
        resumo.Total.Should().Be(5.00m);
    }
}
