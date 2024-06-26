using System.Globalization;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;

namespace Reporting.Domain.Validators;

public class JsonDataValidator : AbstractValidator<string>
{
    public JsonDataValidator()
    {
        RuleFor(x => x)
            .Must(x =>
            {
                if (string.IsNullOrWhiteSpace(x)) return false;
                
                try
                {
                    var doc = JsonDocument.Parse(x);

                    if (doc.RootElement.ValueKind != JsonValueKind.Array) return false;

                    foreach (var element in doc.RootElement.EnumerateArray())
                    {
                        if (element.ValueKind != JsonValueKind.Object) return false;

                        int nameCount = 0;

                        foreach (var prop in element.EnumerateObject())
                        {
                            if (prop.Name == "Name")
                            {
                                ++nameCount;
                                continue;
                            }

                            try
                            {
                                var date = DateTime.ParseExact(prop.Name, "dd.MM.yy", CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {
                                return false;
                            }
                        }

                        if (nameCount != 1) return false;
                    }
                }
                catch (JsonException)
                {
                    return false;
                }

                return true;
            }).WithMessage("Invalid json data");
    }

    protected override void RaiseValidationException(ValidationContext<string> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}