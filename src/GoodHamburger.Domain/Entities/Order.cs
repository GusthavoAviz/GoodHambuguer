using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.ValueObjects;

namespace GoodHamburger.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Product> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OrderSummary CalculateSummary()
    {
        var subtotal = Items.Sum(x => x.Price);
        var discountPercentage = 0m;

        var hasSandwich = Items.Any(x => x.Category == ProductCategory.Sandwich);
        var hasSide = Items.Any(x => x.Category == ProductCategory.Side);
        var hasDrink = Items.Any(x => x.Category == ProductCategory.Drink);

        if (hasSandwich && hasSide && hasDrink)
        {
            discountPercentage = 0.20m;
        }
        else if (hasSandwich && hasDrink)
        {
            discountPercentage = 0.15m;
        }
        else if (hasSandwich && hasSide)
        {
            discountPercentage = 0.10m;
        }

        var discount = subtotal * discountPercentage;
        var total = subtotal - discount;

        return new OrderSummary(subtotal, discount, total);
    }
}
