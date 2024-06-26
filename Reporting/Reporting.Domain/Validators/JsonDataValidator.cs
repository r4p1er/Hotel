using System.Globalization;
using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Hotel.Shared.Exceptions;

namespace Reporting.Domain.Validators;

public class JsonDataValidator : AbstractValidator<JsonDocument>
{
    public JsonDataValidator()
    {
        RuleFor(x => x)
            .Must(x =>
            {
                if (x.RootElement.ValueKind != JsonValueKind.Array) return false;

                bool atLeastOne = false;

                foreach (var element in x.RootElement.EnumerateArray())
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

                    atLeastOne = true;
                }

                return atLeastOne;
            })
            .WithMessage("Invalid json. Json root element must be an array containing at least 1 object comprised of only one Name prop and dates props");
    }

    protected override void RaiseValidationException(ValidationContext<JsonDocument> context, ValidationResult result)
    {
        var ex = new ValidationException(result.Errors);

        throw new BadRequestException(ex.Message);
    }
}