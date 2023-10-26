using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WebApiCore.ComponentModel.Converters;
using WebApiCore.Exceptions;
using WebApiCore.AspNetCore.Middleware.OperationResultExceptionJsonWrapper;

namespace WebApiCore.AspNetCore
{
    public static class OperationResultExceptionJsonWrappingMiddlewareExtension
    {
		/// <summary>
		/// Set configuration for wrapping exceptions by <see cref="OperationResultExceptionJsonWrappingMiddleware"/>
		/// </summary>
		/// <param name="forceAlternativeErrorMsg">Force alternative error message for 'default' exception</param>
		public static OperationResultExceptionJsonWrapperConfiguration AddOperationResultExceptionJsonWrapper<TErrorCodes>(this IServiceCollection services, bool forceAlternativeErrorMsg = false)
			where TErrorCodes : class, IApiErrorCodes, new()
        {
			services.AddSingleton<TErrorCodes>();
			services.AddSingleton<IApiErrorCodes>(s => s.GetRequiredService<TErrorCodes>());
			var cfg = new OperationResultExceptionJsonWrapperConfiguration(forceAlternativeErrorMsg);
			services.AddSingleton(cfg);

			return cfg;
		}

		public static OperationResultExceptionJsonWrapperConfiguration AddOperationResultExceptionJsonWrapper<TErrorCodes>(this IServiceCollection services, Action<TErrorCodes> configureCodes, bool forceAlternativeErrorMsg = false)
            where TErrorCodes : class, IApiErrorCodes, new()
        {
			var errorCodes = new TErrorCodes();
            configureCodes?.Invoke(errorCodes);
            services.AddSingleton(errorCodes);
            services.AddSingleton<IApiErrorCodes>(s => s.GetRequiredService<TErrorCodes>());
            var cfg = new OperationResultExceptionJsonWrapperConfiguration(forceAlternativeErrorMsg);
            services.AddSingleton(cfg);

            return cfg;
        }

        public static OperationResultExceptionJsonWrapperConfiguration AddOperationResultExceptionJsonWrapper<TErrorCodes>(this IServiceCollection services, TErrorCodes errorCodes, bool forceAlternativeErrorMsg = false)
            where TErrorCodes : class, IApiErrorCodes, new()
        {
            services.AddSingleton(errorCodes);
            services.AddSingleton<IApiErrorCodes>(s => s.GetRequiredService<TErrorCodes>());
            var cfg = new OperationResultExceptionJsonWrapperConfiguration(forceAlternativeErrorMsg);
            services.AddSingleton(cfg);

            return cfg;
        }

        /// <summary>
        /// Wrap exceptions as OperationResult json
        /// </summary>
        public static IApplicationBuilder UseOperationResultExceptionWrapper(this IApplicationBuilder app)
		{
			var options = new JsonSerializerOptions();
			options.SetApiSerializationOptions();

			app.UseMiddleware<OperationResultExceptionJsonWrappingMiddleware>(options);

			return app;
		}

		/// <summary>
		/// Wrap exceptions as OperationResult json
		/// </summary>
		public static IApplicationBuilder UseOperationResultExceptionWrapper(this IApplicationBuilder app, Action<JsonSerializerOptions> configure)
		{
			var options = new JsonSerializerOptions();
			options.SetApiSerializationOptions();

			configure?.Invoke(options);

			app.UseMiddleware<OperationResultExceptionJsonWrappingMiddleware>(options);

			return app;
		}

		/// <summary>
		/// Wrap exceptions as OperationResult json
		/// </summary>
		public static IApplicationBuilder UseOperationResultExceptionWrapper(this IApplicationBuilder app, JsonSerializerOptions options)
		{
			app.UseMiddleware<OperationResultExceptionJsonWrappingMiddleware>(options);

			return app;
		}

		private static void SetApiSerializationOptions(this JsonSerializerOptions options)
		{
			options.Converters.Add(new StringEnumJsonConverterFactory());

			options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			options.WriteIndented = true;
		}
	}
}
