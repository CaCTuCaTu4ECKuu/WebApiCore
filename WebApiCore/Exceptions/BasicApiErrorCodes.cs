using System;
using System.Collections.Generic;

namespace WebApiCore.Exceptions
{
	/// <summary>
	/// Base for storing application managed errors description
	/// </summary>
    public abstract class BasicApiErrorCodes : IApiErrorCodes
    {
		private readonly Dictionary<int, int> http_codes;

		private readonly Dictionary<int, string> error_messages;

		#region SERVICE ERRORS /0-99/
		/// <summary>
		/// Неизвестная ошибка
		/// </summary>
		public const int UNKNOWN_ERROR = 0;

		/// <summary>
		/// Ошибка сервера (предусмотренная)
		/// </summary>
		public const int INTERNAL_SERVER_ERROR = 1;

		/// <summary>
		/// В данный момент не реализовано
		/// </summary>
		public const int NOT_IMPLEMENTED = 2;

		/// <summary>
		/// Ошибка связанна с таймаутом
		/// </summary>
		public const int TIMEOUT = 3;
		#endregion

		#region "CLIENT" ERRORS /100 - 199/
		/// <summary>
		/// Проблема с тем, что пришло в запросе
		/// </summary>
		public const int BAD_REQUEST = 100;

		/// <summary>
		/// Для выполнения нужно авторизоваться
		/// </summary>
		public const int UNAUTHORIZED = 101;

		/// <summary>
		/// Выполнение запрещено
		/// </summary>
		public const int FORBIDDEN = 103;

		public const int ACCESS_TOKEN_ERROR = 140;
		public const int ACCESS_TOKEN_EXPIRED = 141;
		public const int ACCESS_TOKEN_INVALID = 142;
		#endregion

		#region OTHER COMMON /200 - 300/
		/// <summary>
		/// Не поддерживается
		/// </summary>
		public const int NOT_SUPPORTED = 200;

		/// <summary>
		/// Версия API не поддерживается
		/// </summary>
		public const int NOT_SUPPORTED_API_VERSION = 201;
		#endregion

		protected Dictionary<int, int> GetHttpCodes(Dictionary<int, int> httpCodes)
        {
			var res = new Dictionary<int, int>()
			{
				{ 0, 500 }, // Internal Server Error
				{ 1, 500 }, // Internal Server Error
				{ 2, 501 }, // Not Implemented
				{ 3, 500 }, // Internal Server Error

				{ 100, 400 }, // Bad request
				{ 101, 401 }, // Unauthorized
				{ 103, 403 }, // Forbidden
				{ 140, 500 }, // Internal Server Error
				{ 141, 401 }, // Unauthorized
				{ 142, 400 }, // Bad request
				
				{ 200, 501 }, // Not Implemented
				{ 201, 400 }, // Bad request
			};

			if (httpCodes != null)
			{
				foreach (var code in httpCodes)
					res[code.Key] = code.Value;
			}

			return res;
		}

		protected Dictionary<int, string> GetErrorMessages(Dictionary<int, string> errorMessages)
        {
			var res = new Dictionary<int, string>()
			{
				{ 0, "Unknown Server Error" }, // Internal Server Error
				{ 1, "Internal Server Error" }, // Internal Server Error
				{ 2, "Not Implemented" }, // Not Implemented
				{ 3, "Timeout" }, // Internal Server Error

				{ 100, "Bad request" }, // Bad request
				{ 101, "Unauthorized" }, // Unauthorized
				{ 103, "Forbidden" }, // Forbidden
				{ 140, "Access Token Error" }, // Internal Server Error
				{ 141, "Access Token Expired" }, // Unauthorized
				{ 142, "Access Token is invalid" }, // Bad request
				
				{ 200, "Operation is not supported" }, // Not Implemented
				{ 201, "API version is not supported" }, // Bad request
			};

			if (errorMessages != null)
            {
				foreach (var error in errorMessages)
					res[error.Key] = error.Value;
            }

			return res;
		}

		public BasicApiErrorCodes()
		{
			http_codes = GetHttpCodes(null);
			error_messages = GetErrorMessages(null);
		}

		/// <param name="httpCodes">Set of error codes and HTTP response codes for result</param>
		/// <param name="errorMessages">Set of error codes and default error messages</param>
		/// <exception cref="ArgumentNullException"></exception>
		public BasicApiErrorCodes(Dictionary<int, int> httpCodes, Dictionary<int, string> errorMessages)
        {
			httpCodes = GetHttpCodes(httpCodes);
			error_messages = GetErrorMessages(errorMessages);
        }

		/// <summary>
		/// Define HTTP response code and response error message for error code.
		/// </summary>
		/// <param name="errorCode">Application error code</param>
		/// <param name="httpCode">HTTP response code</param>
		/// <param name="errorMessage">Response error message</param>
		protected void SetCode(int errorCode, int httpCode, string errorMessage)
		{
			http_codes[errorCode] = httpCode;
			error_messages[errorCode] = errorMessage;
		}
		/// <summary>
		/// Remove application error from error codes collection
		/// </summary>
		/// <param name="errorCode">Application error code</param>
		protected void ClearCode(int errorCode)
		{
			if (http_codes.ContainsKey(errorCode))
			{
				http_codes.Remove(errorCode);
				error_messages.Remove(errorCode);
			}
		}

		///<inheritdoc/>
		public bool HasCode(int errorCode) => http_codes.ContainsKey(errorCode);

		///<inheritdoc/>
		public int GetHTTPResponseCode(int errorCode) => http_codes[errorCode];

		///<inheritdoc/>
		public string GetErrorMessage(int errorCode) => error_messages[errorCode];

		///<inheritdoc/>
		public string GetUnknownErrorMessage() => GetErrorMessage(UNKNOWN_ERROR);
	}
}
