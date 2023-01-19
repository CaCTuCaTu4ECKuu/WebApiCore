using System;
using System.Linq;
using FluentValidation;

namespace WebApiCore.FluentValidation.Validators.UriValidators
{
    public class HostAddressUriValidator<T> : UriStringValidator<T>
    {
        public override string Name => "HostAddressUriValidator";

        private string[] _hosts;
        private string _serviceName;

        public HostAddressUriValidator(string host, string serviceName = null) : base(UriKind.Absolute)
        {
            host = host?.Trim()?.ToLower();
            if (string.IsNullOrEmpty(host))
            {
                if (host == null)
                    throw new ArgumentNullException(nameof(host));
                else
                    throw new ArgumentException("Uri host address not defined.", nameof(host));
            }

            _hosts = new[] { host };
            _serviceName = serviceName;
        }
        public HostAddressUriValidator(string[] hosts, string serviceName = null) : base(UriKind.Absolute)
        {
            if (hosts == null)
                throw new ArgumentNullException(nameof(hosts));

            hosts = hosts.Where(h => !string.IsNullOrEmpty(h))
                .Select(h => h?.Trim()?.ToLower())
                .ToArray();

            if (!hosts.Any())
                throw new ArgumentException("Uri host addresses not defined.", nameof(hosts));

            _hosts = hosts;
            _serviceName = serviceName;
        }

        protected override bool ValidateAddress(ValidationContext<T> context, Uri uri)
        {
            if (_hosts.Any(h => h.EndsWith(uri.Host, StringComparison.OrdinalIgnoreCase)))
                return true;

            if (string.IsNullOrEmpty(_serviceName))
            {
                if (_hosts.Length == 1)
                    context.AddFailure(context.PropertyName, $"'{{PropertyName}}' with value '{{PropertyValue}}' is not under '{_hosts[0]}' domain.");
                else
                    context.AddFailure(context.PropertyName, $"'{{PropertyName}}' with value '{{PropertyValue}}' is not under any of host domains.");
            }
            else
                context.AddFailure(context.PropertyName, $"'{{PropertyName}}' with value '{{PropertyValue}}' is not '{_serviceName}' Uri.");

            return false;
        }
    }
}
