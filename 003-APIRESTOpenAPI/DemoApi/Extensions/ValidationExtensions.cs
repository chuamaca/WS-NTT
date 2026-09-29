using System.ComponentModel.DataAnnotations;

namespace DemoApi.Extensions;

public static class ValidationExtensions
{
    public static List<string> GetValidationErrors(this object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model);

        Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);

        if (validationResults.Count > 0 && model is IValidatableObject validatable)
            validationResults.AddRange(validatable.Validate(context));

        return validationResults
            .Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage))
            .Select(r => r.ErrorMessage!)
            .Distinct()
            .ToList();
    }
}