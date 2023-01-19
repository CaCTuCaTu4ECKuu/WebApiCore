using System;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace WebApiCore.FluentValidation.Validators
{
    public class EmailStringValidator<T> : PropertyValidator<T, string>
    {
        /// <summary>
        /// RFC 5322 Internet Message Format
        /// </summary>
        public const string RFC5322_EMAIL_REGEX_PATTERN = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z_]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z_])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        protected static Regex Rfc5322EmailRegex = new Regex(RFC5322_EMAIL_REGEX_PATTERN, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public override string Name => "EmailStringValidator";

        protected readonly Regex _emailRegex;

        public EmailStringValidator(string emailRegexPattern = null)
        {
            if (emailRegexPattern == null || emailRegexPattern.Equals(RFC5322_EMAIL_REGEX_PATTERN))
                _emailRegex = Rfc5322EmailRegex;
            else
            {
                try
                {
                    _emailRegex = new Regex(emailRegexPattern, RegexOptions.IgnoreCase);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Invalid regular expression.", nameof(emailRegexPattern), ex);
                }
            }
        }

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (value == null)
                    return true;

                context.AddFailure(context.PropertyName, "'{PropertyName}' should not be empty.");
            }

            if (_emailRegex.IsMatch(value))
                return true;

            context.AddFailure(context.PropertyName, "'{PropertyName}' with value '{PropertyValue}' is not valid email address.");
            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' value should be RFC 5322 compliant email address.";
    }

    public static class EmailStringValidatorExtension
    {
        /// <summary>
        /// Ensure that propery is valid email address
        /// </summary>
        /// <param name="emailRegexPattern">Regular expression to check email addresses</param>
        public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder, string emailRegexPattern)
        {
            return ruleBuilder.SetValidator(new EmailStringValidator<T>(emailRegexPattern));
        }

        /// <summary>
        /// Ensure that propery is RFC 5322 compliant email address
        /// </summary>
        /// <param name="emailRegexPattern">Regular expression to check email addresses</param>
        public static IRuleBuilderOptions<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new EmailStringValidator<T>());
        }
    }
}
