using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Validadores;
using GoodHamburger.Domain.Enums;
using Xunit;

namespace GoodHamburger.Tests;

public class PedidoValidadorTestes
{
    private readonly CriarPedidoValidador _validador = new();

    [Fact]
    public void Validar_ComSanduicheDuplicado_DeveTerErroValidacao()
    {
        // Arrange
        var requisicao = new CriarPedidoRequisicao(new List<ItemPedidoRequisicao>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, CategoriaProduto.Sanduiche),
            new(Guid.NewGuid(), "X Egg", 4.50m, CategoriaProduto.Sanduiche)
        });

        // Act
        var resultado = _validador.Validate(requisicao);

        // Assert
        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(x => x.ErrorMessage == "O pedido pode conter apenas um sanduíche.");
    }
}
