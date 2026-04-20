using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.ValueObjects;

public record OrderSummary(decimal Subtotal, decimal Discount, decimal Total);
