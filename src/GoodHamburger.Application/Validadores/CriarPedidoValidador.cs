using FluentValidation;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.Validadores;

/// <summary>
/// Validador para a criação de pedidos, garantindo as restrições de unicidade por categoria.
/// </summary>
public class CriarPedidoValidador : AbstractValidator<CriarPedidoRequisicao>
{
    public CriarPedidoValidador()
    {
        RuleFor(x => x.Itens)
            .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

        RuleFor(x => x.Itens)
            .Must(itens => itens.Count(i => i.Categoria == CategoriaProduto.Sanduiche) <= 1)
            .WithMessage("O pedido pode conter apenas um sanduíche.")
            .Must(itens => itens.Count(i => i.Categoria == CategoriaProduto.Acompanhamento) <= 1)
            .WithMessage("O pedido pode conter apenas uma batata.")
            .Must(itens => itens.Count(i => i.Categoria == CategoriaProduto.Bebida) <= 1)
            .WithMessage("O pedido pode conter apenas um refrigerante.");
    }
}
