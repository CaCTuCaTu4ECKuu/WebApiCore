using System;
using FluentValidation;
using FluentValidation.Validators;

namespace WebApiCore.FluentValidation.Validators
{
    public class ValidGuidStringValidator<T> : PropertyValidator<T, string>
    {
        protected static string EMPTY_GUID = Guid.Empty.ToString();

        public override string Name => "ValidGuidStringValidator";

        protected readonly bool _validateEmptyGuid;

        public ValidGuidStringValidator(bool validateEmptyGuid = true)
        {
            _validateEmptyGuid = validateEmptyGuid; 
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (value == null)
                    return true;

                context.AddFailure(context.PropertyName, "'{PropertyName}' should not be empty.");
                return false;
            }

            if (value.Equals(EMPTY_GUID, StringComparison.OrdinalIgnoreCase))
            {
                context.AddFailure(context.PropertyName, "'{PropertyName}' is not allowed to be empty Guid.");
                return _validateEmptyGuid;
            }

            if (Guid.TryParse(value, out _))
                return true;

            context.AddFailure(context.PropertyName, "'{PropertyName}' with value '{PropertyValue}' should be Guid type.");
            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' value should be Guid.";
    }

    public static class ValidGuidStringValidatorExtension
    {
        /// <summary>
        /// Ensure that propery is valid Guid if it is presented
        /// </summary>
        public static IRuleBuilderOptions<T, string> IsValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new ValidGuidStringValidator<T>(true));
        }

        /// <summary>
        /// Ensure that propery is valid Guid if it is presented
        /// </summary>
        /// <param name="validateEmptyGuid">Allow property to have empty Guid value</param>
        public static IRuleBuilderOptions<T, string> IsValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder, bool validateEmptyGuid)
        {
            return ruleBuilder.SetValidator(new ValidGuidStringValidator<T>(validateEmptyGuid));
        }
    }
}
