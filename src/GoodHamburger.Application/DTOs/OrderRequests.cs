using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs;

public record OrderItemRequest(Guid ProductId, string Name, decimal Price, ProductCategory Category);

public record CreateOrderRequest(List<OrderItemRequest> Items);
