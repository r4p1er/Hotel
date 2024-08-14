using FluentValidation;
using FluentValidation.Results;
using Hotel.Managing.Domain.DataObjects;
using Hotel.Shared.Exceptions;

namespace Hotel.Managing.Domain.Validators;

/// <summary>
/// Валидатор данных нового номера отеля
/// </summary>
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

    /// <summary>
    /// Изменить стандартное исключение на свое кастомное
    /// </summary>
    /// <param name="context">Контекст валидации</param>
    /// <param name="result">Результат валидации</param>
    /// <exception cref="BadRequestException">Исключение с HTTP статус кодом 400 Bad Request</exception>
    protected override void RaiseValidationException(ValidationContext<RoomData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}