using Booking.Domain.DataObjects;
using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;

namespace Booking.Domain.Validators;

public class TicketDataValidator : AbstractValidator<TicketData>
{
    public TicketDataValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty");
        
        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0");
        
        RuleFor(x => x.To)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty");

        RuleFor(x => x.From)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("{PropertyName} must not be lower than the current date and time")
            .Must((data, from) => from < data.To)
            .WithMessage("{PropertyName} must not be greater than the property To");
    }

    protected override void RaiseValidationException(ValidationContext<TicketData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}