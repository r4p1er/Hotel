using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;
using Identity.Domain.DataObjects;

namespace Identity.Domain.Validators;

/// <summary>
/// Валидатор регистрационных данных
/// </summary>
public class RegisterDataValidator : AbstractValidator<RegisterData>
{
    public RegisterDataValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(255)
            .WithMessage("{PropertyName} length must not be greater than 255");
        
        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(255)
            .WithMessage("{PropertyName} length must not be greater than 255");
        
        RuleFor(x => x.Patronymic)
            .MaximumLength(255)
            .When(x => x.Patronymic != null)
            .WithMessage("{PropertyName} length must not be greater than 255");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(50)
            .WithMessage("{PropertyName} length must not be greater than 50")
            .EmailAddress()
            .WithMessage("{PropertyName} must be a valid email address");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(14)
            .WithMessage("{PropertyName} length must not be greater than 14");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(128)
            .WithMessage("{PropertyName} length must not be greater than 128");
    }

    /// <summary>
    /// Изменить стандартное исключение на свое кастомное
    /// </summary>
    /// <param name="context">Контекст валидации</param>
    /// <param name="result">Результат валидации</param>
    /// <exception cref="BadRequestException">Исключение с HTTP статус кодом 400 Bad Request</exception>
    protected override void RaiseValidationException(ValidationContext<RegisterData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}