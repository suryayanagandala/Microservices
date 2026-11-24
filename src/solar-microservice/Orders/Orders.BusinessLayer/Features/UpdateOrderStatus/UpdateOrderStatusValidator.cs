using FluentValidation;

namespace Orders.BusinessLayer.Features.UpdateOrderStatus;

public sealed class UpdateOrderStatusValidator
    : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusValidator()
    {
        RuleFor(command => command.OrderId).GreaterThan(0);

        RuleFor(command => command.Status).IsInEnum();
    }
}
