using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Validators;
using GoodHamburger.Domain.Enums;
using Xunit;

namespace GoodHamburger.Tests;

public class OrderValidatorTests
{
    private readonly CreateOrderValidator _validator = new();

    [Fact]
    public void Validate_WithDuplicateSandwich_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateOrderRequest(new List<OrderItemRequest>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, ProductCategory.Sandwich),
            new(Guid.NewGuid(), "X Egg", 4.50m, ProductCategory.Sandwich)
        });

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "O pedido pode conter apenas um sanduíche.");
    }

    [Fact]
    public void Validate_WithValidItems_ShouldBeValid()
    {
        // Arrange
        var request = new CreateOrderRequest(new List<OrderItemRequest>
        {
            new(Guid.NewGuid(), "X Burger", 5.00m, ProductCategory.Sandwich),
            new(Guid.NewGuid(), "Batata frita", 2.00m, ProductCategory.Side),
            new(Guid.NewGuid(), "Refrigerante", 2.50m, ProductCategory.Drink)
        });

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
