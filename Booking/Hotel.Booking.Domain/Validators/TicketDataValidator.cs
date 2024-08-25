using FluentValidation;
using FluentValidation.Results;
using Hotel.Booking.Domain.Models;
using Hotel.Shared.Exceptions;

namespace Hotel.Booking.Domain.Validators;

/// <summary>
/// Валидатор данных для создания новой заявки на бронирование
/// </summary>
public class TicketDataValidator : AbstractValidator<TicketData>
{
    /// <summary/>
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

    /// <summary>
    /// Изменение стандартного exception на свой кастомный
    /// </summary>
    /// <param name="context">Контекст валидации</param>
    /// <param name="result">Результат валидации</param>
    /// <exception cref="BadRequestException">Исключение с HTTP статус кодом 400 Bad Request</exception>
    protected override void RaiseValidationException(ValidationContext<TicketData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}