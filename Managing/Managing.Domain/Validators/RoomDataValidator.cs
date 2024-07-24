using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;
using Managing.Domain.DataObjects;

namespace Managing.Domain.Validators;

public class RoomDataValidator : AbstractValidator<RoomData>
{
    public RoomDataValidator()
    {
        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(255)
            .WithMessage("{PropertyName} length must not be greater than 255");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(255)
            .WithMessage("{PropertyName} length must not be greater than 255");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0");
    }

    protected override void RaiseValidationException(ValidationContext<RoomData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}