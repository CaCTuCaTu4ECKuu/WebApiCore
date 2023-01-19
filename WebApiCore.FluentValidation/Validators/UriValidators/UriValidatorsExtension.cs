using FluentValidation;

namespace WebApiCore.FluentValidation.Validators.UriValidators
{
    public static class UriValidatorsExtension
    {
        public static string[] YTHosts = new[] { "youtube.com" };
        public static string[] VkHosts = new[] { "vk.com" };


        public static IRuleBuilderOptions<T, string> IsYouTubeUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new HostAddressUriValidator<T>(YTHosts, "YouTube"));
        }
        public static IRuleBuilderOptions<T, string> IsVkontakteUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new HostAddressUriValidator<T>(VkHosts, "VKontakte"));
        }
    }
}
