using FluentAssertions;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Xunit;

namespace GoodHamburger.Tests;

public class OrderTests
{
    [Fact]
    public void CalculateSummary_WithSandwichSideAndDrink_ShouldApply20PercentDiscount()
    {
        // Arrange
        var order = new Order();
        order.Items.Add(new Product { Name = "X Burger", Price = 5.00m, Category = ProductCategory.Sandwich });
        order.Items.Add(new Product { Name = "Batata frita", Price = 2.00m, Category = ProductCategory.Side });
        order.Items.Add(new Product { Name = "Refrigerante", Price = 2.50m, Category = ProductCategory.Drink });

        // Act
        var summary = order.CalculateSummary();

        // Assert
        // Subtotal: 5 + 2 + 2.5 = 9.5
        // Discount: 20% of 9.5 = 1.9
        // Total: 9.5 - 1.9 = 7.6
        summary.Subtotal.Should().Be(9.50m);
        summary.Discount.Should().Be(1.90m);
        summary.Total.Should().Be(7.60m);
    }

    [Fact]
    public void CalculateSummary_WithSandwichAndDrink_ShouldApply15PercentDiscount()
    {
        // Arrange
        var order = new Order();
        order.Items.Add(new Product { Name = "X Burger", Price = 5.00m, Category = ProductCategory.Sandwich });
        order.Items.Add(new Product { Name = "Refrigerante", Price = 2.50m, Category = ProductCategory.Drink });

        // Act
        var summary = order.CalculateSummary();

        // Assert
        // Subtotal: 5 + 2.5 = 7.5
        // Discount: 15% of 7.5 = 1.125
        // Total: 7.5 - 1.125 = 6.375
        summary.Subtotal.Should().Be(7.50m);
        summary.Discount.Should().Be(1.125m);
        summary.Total.Should().Be(6.375m);
    }

    [Fact]
    public void CalculateSummary_WithSandwichAndSide_ShouldApply10PercentDiscount()
    {
        // Arrange
        var order = new Order();
        order.Items.Add(new Product { Name = "X Burger", Price = 5.00m, Category = ProductCategory.Sandwich });
        order.Items.Add(new Product { Name = "Batata frita", Price = 2.00m, Category = ProductCategory.Side });

        // Act
        var summary = order.CalculateSummary();

        // Assert
        // Subtotal: 5 + 2 = 7
        // Discount: 10% of 7 = 0.7
        // Total: 7 - 0.7 = 6.3
        summary.Subtotal.Should().Be(7.00m);
        summary.Discount.Should().Be(0.70m);
        summary.Total.Should().Be(6.30m);
    }

    [Fact]
    public void CalculateSummary_WithOnlySandwich_ShouldApplyNoDiscount()
    {
        // Arrange
        var order = new Order();
        order.Items.Add(new Product { Name = "X Burger", Price = 5.00m, Category = ProductCategory.Sandwich });

        // Act
        var summary = order.CalculateSummary();

        // Assert
        summary.Subtotal.Should().Be(5.00m);
        summary.Discount.Should().Be(0m);
        summary.Total.Should().Be(5.00m);
    }
}
