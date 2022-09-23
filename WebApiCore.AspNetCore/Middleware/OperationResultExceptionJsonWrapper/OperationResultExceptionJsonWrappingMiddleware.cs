using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

using WebApiCore.ComponentModel;
using WebApiCore.Exceptions;

namespace WebApiCore.AspNetCore.Middleware.OperationResultExceptionJsonWrapper
{
    public class OperationResultExceptionJsonWrappingMiddleware
    {
		private readonly RequestDelegate _next;
		private readonly JsonSerializerOptions _options;
		private readonly OperationResultExceptionJsonWrapperConfiguration _config;
		private readonly IApiErrorCodes _apiErrorCodes;

		public OperationResultExceptionJsonWrappingMiddleware(RequestDelegate next, JsonSerializerOptions options,
			OperationResultExceptionJsonWrapperConfiguration config,
			IApiErrorCodes apiErrorCodes)
        {
            _next = next;
            _options = options;
            _config = config;
            _apiErrorCodes = apiErrorCodes;
        }

        public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				var exType = ex.GetType();
				if (_config.Exceptions.ContainsKey(exType))
                {
					var opErrCfg = _config.Exceptions[exType];
					var opErr = opErrCfg.CreateError(ex, _apiErrorCodes);
					await SendResponse(context, opErr, opErrCfg.ResponseStatusCode ?? _apiErrorCodes.GetHTTPResponseCode(opErrCfg.ErrorCode));
                }
				else
				{
					var opErr = new OperationError(_config.Default.ErrorCode, _config.Default.ForceErrorMessage
						? _config.Default.AlternativeErrorMessage
						: ex.Message ?? _config.Default.AlternativeErrorMessage);
					await SendResponse(context, opErr, _config.Default.ResponseStatusCode ?? StatusCodes.Status500InternalServerError);
				}
			}
		}

		private Task SendResponse(HttpContext context, OperationError error, int statusCode)
		{
			var res = OperationResult<object>.Failed(error);
			var json = JsonSerializer.Serialize(res, _options);
			context.Response.StatusCode = statusCode;
			context.Response.ContentType = "application/json";

			return context.Response.WriteAsync(json);
		}
	}
}
