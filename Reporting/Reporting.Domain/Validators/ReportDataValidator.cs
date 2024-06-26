using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;
using Reporting.Domain.DataObjects;

namespace Reporting.Domain.Validators;

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

        RuleFor(x => x.Data)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty");
    }

    protected override void RaiseValidationException(ValidationContext<ReportData> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}