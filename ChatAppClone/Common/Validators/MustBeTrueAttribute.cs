namespace ChatAppClone.Common.Validators
{
    using System.ComponentModel.DataAnnotations;

    public class MustBeTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is bool && (bool)value)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "You must accept the terms and conditions.");
        }
    }
}
