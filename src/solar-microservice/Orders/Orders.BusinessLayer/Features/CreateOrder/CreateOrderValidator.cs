using FluentValidation;

namespace Orders.BusinessLayer.Features.CreateOrder;

public sealed class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(command => command.DeliveryAddress)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(command => command.PhoneNumber)
            .Matches("^[0-9]{10}$")
            .When(command => !string.IsNullOrWhiteSpace(command.PhoneNumber))
            .WithMessage("Phone number must contain exactly 10 digits.");

        RuleFor(command => command.FullName)
            .MaximumLength(255)
            .When(command => !string.IsNullOrWhiteSpace(command.FullName));

        RuleFor(command => command.Items)
            .NotEmpty()
            .WithMessage("At least one order item is required.");

        RuleForEach(command => command.Items).ChildRules(item =>
        {
            item.RuleFor(value => value.ProductId).GreaterThan(0);
            item.RuleFor(value => value.Quantity).GreaterThan(0);
        });
    }
}
