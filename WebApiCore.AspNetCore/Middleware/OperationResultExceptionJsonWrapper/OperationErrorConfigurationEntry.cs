using System;
using Microsoft.AspNetCore.Http;
using WebApiCore.ComponentModel;
using WebApiCore.Exceptions;

namespace WebApiCore.AspNetCore.Middleware.OperationResultExceptionJsonWrapper
{
    public class OperationErrorConfigurationEntry
    {
        /// <summary>
        /// Error code for <see cref="OperationError"/>
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// <see cref="StatusCodes"/> for HTTP response
        /// </summary>
        public int? ResponseStatusCode { get; }

        /// <summary>
        /// Error message for <see cref="OperationError"/> if exception error message is not presented
        /// </summary>
        public string AlternativeErrorMessage { get; }

        /// <summary>
        /// Use <see cref="AlternativeErrorMessage"/> instead exception error message
        /// </summary>
        public bool ForceErrorMessage { get; set; }

        /// <summary>
        /// Function that allow to get error message based on content of exception
        /// </summary>
        public Func<Exception, IApiErrorCodes, string> ExceptionErrorMessageParser { get; } = null;

        /// <summary></summary>
        /// <param name="errorCode">Error code for <see cref="OperationError"/></param>
        /// <param name="statusCode"><see cref="StatusCodes"/> for HTTP response</param>
        /// <param name="alternativeMsg">Error message if exception error message is not presented</param>
        /// <param name="forceErrorMsg">Force alternative error message instead exception error message</param>
        public OperationErrorConfigurationEntry(int errorCode, int? statusCode, string alternativeMsg, bool forceErrorMsg = false)
        {
            ErrorCode = errorCode;
            ResponseStatusCode = statusCode;
            if (string.IsNullOrEmpty(alternativeMsg))
                throw new ArgumentException("Alternative error message must be defined", nameof(alternativeMsg));
            AlternativeErrorMessage = alternativeMsg;
            ForceErrorMessage = forceErrorMsg;
        }

        /// <summary></summary>
        /// <param name="statusCode"><see cref="StatusCodes"/> for HTTP response</param>
        /// <param name="errorCode">Error code for <see cref="OperationError"/></param>
        /// <param name="exceptionMsgParser">Exception message parser for <see cref="OperationError.Message"/></param>
        public OperationErrorConfigurationEntry(int errorCode, int? statusCode, Func<Exception, IApiErrorCodes, string> exceptionMsgParser)
        {
            ErrorCode = errorCode;
            ResponseStatusCode = statusCode;
            ExceptionErrorMessageParser = exceptionMsgParser;
        }

        /// <summary>
        /// Create <see cref="OperationError"/> using configuration
        /// </summary>
        /// <param name="ex">Raised exception</param>
        /// <returns></returns>
        public OperationError CreateError(Exception ex, IApiErrorCodes errorCodes)
        {
            if (ExceptionErrorMessageParser != null)
                return new OperationError(ErrorCode, ExceptionErrorMessageParser(ex, errorCodes));

            if (ForceErrorMessage)
                return new OperationError(ErrorCode, AlternativeErrorMessage);

            return new OperationError(ErrorCode, string.IsNullOrEmpty(ex.Message) ? AlternativeErrorMessage : ex.Message);
        }
    }
}
