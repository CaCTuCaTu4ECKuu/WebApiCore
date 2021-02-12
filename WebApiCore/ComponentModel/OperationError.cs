using System.Diagnostics;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebApiCore.ComponentModel
{
	[DebuggerDisplay("{Code} - {Message}")]
    public class OperationError
    {
		[JsonPropertyName("code")]
		[JsonProperty("code")]
		public int Code { get; protected set; }

		[JsonPropertyName("msg")]
		[JsonProperty("msg")]
		public string Message { get; protected set; }

		public OperationError(int errorCode)
		{
			Code = errorCode;
		}

		public OperationError(int errorCode, string errorMessage)
		{
			Code = errorCode;
			Message = errorMessage;
		}
	}
}
