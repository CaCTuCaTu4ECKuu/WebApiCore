using System;
using FluentValidation;
using FluentValidation.Validators;

namespace WebApiCore.FluentValidation.Validators
{
    public class UriStringValidator<T> : PropertyValidator<T, string>
    {
        public override string Name => "UriStringValidator";

        private UriKind _uriKind;

        public UriStringValidator(UriKind uriKind)
        {
            _uriKind = uriKind;
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            if (Uri.TryCreate(value, _uriKind, out Uri uri))
                return ValidateAddress(context, uri);

            context.AddFailure(context.PropertyName, "'{PropertyName}' with value '{PropertyValue}' is not valid Uri.");
            return false;
        }

        protected virtual bool ValidateAddress(ValidationContext<T> context, Uri uri)
        {
            return true;
        }
    }

    public static class UriStringValidatorExtension
    {
        public static IRuleBuilderOptions<T, string> IsValidUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriStringValidator<T>(UriKind.RelativeOrAbsolute));
        }

        public static IRuleBuilderOptions<T, string> IsValidAbsoluteUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriStringValidator<T>(UriKind.Absolute));
        }

        public static IRuleBuilderOptions<T, string> IsValidRelativeUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UriStringValidator<T>(UriKind.Relative));
        }
    }
}
