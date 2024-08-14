using FluentValidation;
using FluentValidation.Results;
using Hotel.Reporting.Domain.Models;
using Hotel.Shared.Exceptions;

namespace Hotel.Reporting.Domain.Validators;

/// <summary>
/// Валидатор данных для создания отчета
/// </summary>
public class ReportDataValidator : AbstractValidator<ReportData>
{
    public ReportDataValidator()
    {
        RuleFor(x => x.Summary)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .MaximumLength(255)
            .WithMessage("{PropertyName} length must not be greater than 255");

        RuleFor(x => x.To)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty");

        RuleFor(x => x.From)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty")
            .Must((data, from) => from < data.To)
            .WithMessage("{PropertyName} must not be greater than the property To");
    }

    /// <summary>
    /// Изменить стандартное исключение на свое кастомное
    /// </summary>
    /// <param name="context">Контекст валидации</param>
    /// <param name="result">Результат валидации</param>
    /// <exception cref="BadRequestException">Исключение с HTTP статус кодом 400 Bad Request</exception>
    protected override void RaiseValidationException(ValidationContext<ReportData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}