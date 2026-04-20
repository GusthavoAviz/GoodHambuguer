using FluentValidation;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

        RuleFor(x => x.Items)
            .Must(items => items.Count(i => i.Category == ProductCategory.Sandwich) <= 1)
            .WithMessage("O pedido pode conter apenas um sanduíche.")
            .Must(items => items.Count(i => i.Category == ProductCategory.Side) <= 1)
            .WithMessage("O pedido pode conter apenas uma batata.")
            .Must(items => items.Count(i => i.Category == ProductCategory.Drink) <= 1)
            .WithMessage("O pedido pode conter apenas um refrigerante.");
    }
}
