using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebApiCore.ComponentModel;
using WebApiCore.Exceptions;

namespace WebApiCore.AspNetCore.Middleware.OperationResultExceptionJsonWrapper
{
    public class OperationResultExceptionJsonWrapperConfiguration
    {
        private readonly Dictionary<Type, OperationErrorConfigurationEntry> _exceptions;

        /// <summary>
        /// List of wrapper configuration for exception types
        /// </summary>
        public IReadOnlyDictionary<Type, OperationErrorConfigurationEntry> Exceptions => _exceptions;

        /// <summary>
        /// Configuration for handling <see cref="NotImplementedException"/>
        /// </summary>
        public OperationErrorConfigurationEntry NotImplemented
        {
            get => _exceptions[typeof(NotImplementedException)];
            set => _exceptions[typeof(NotImplementedException)] = value;
        }

        /// <summary>
        /// Configuration for handling exception when no other configuration for that type
        /// </summary>
        public OperationErrorConfigurationEntry Default { get; set; }

        /// <summary>
        /// Creates configuration for <see cref="OperationResultExceptionJsonWrappingMiddleware"/>
        /// with default wrapper for <see cref="Exception"/> and <see cref="NotImplementedException"/>
        /// </summary>
        /// <param name="forceAlternativeErrorMsg">Force to set defined error message even if exception contains error message</param>
        public OperationResultExceptionJsonWrapperConfiguration(bool forceAlternativeErrorMsg = false)
        {
            _exceptions = new Dictionary<Type, OperationErrorConfigurationEntry>();

            Default = new OperationErrorConfigurationEntry(BasicApiErrorCodes.UNKNOWN_ERROR, StatusCodes.Status500InternalServerError, "Unknown Server Error", forceAlternativeErrorMsg);
            NotImplemented = new OperationErrorConfigurationEntry(BasicApiErrorCodes.NOT_IMPLEMENTED, StatusCodes.Status501NotImplemented, "Not Implemented", forceAlternativeErrorMsg);
        }

        /// <summary>
        /// Add or remove (if config is null) exception wrapper configuration
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="config"></param>
        public OperationResultExceptionJsonWrapperConfiguration SetExceptionWrapper(Type exceptionType, OperationErrorConfigurationEntry config)
        {
            if (config != null)
                _exceptions[exceptionType] = config;
            else if (_exceptions.ContainsKey(exceptionType))
                _exceptions.Remove(exceptionType);

            return this;
        }

        /// <summary>Add exception wrapper configuration</summary>
        /// <param name="statusCode"><see cref="StatusCodes"/> for HTTP response</param>
        /// <param name="errorCode">Error code for <see cref="OperationError"/></param>
        /// <param name="alternativeMsg">Error message if exception error message is not presented</param>
        /// <param name="forceErrorMsg">Force alternative error message instead exception error message</param>
        public OperationResultExceptionJsonWrapperConfiguration SetExceptionWrapper(Type exceptionType, int statusCode, int errorCode, string alternativeMsg, bool forceErrorMsg = false)
        {
            var cfg = new OperationErrorConfigurationEntry(errorCode, statusCode, alternativeMsg, forceErrorMsg);
            SetExceptionWrapper(exceptionType, cfg);
            
            return this;
        }

        /// <summary>Add exception wrapper configuration</summary>
        /// <param name="exceptionType"></param>
        /// <param name="statusCode"><see cref="StatusCodes"/> for HTTP response</param>
        /// <param name="errorCode">Error code for <see cref="OperationError"/></param>
        /// <param name="exceptionMsgParser">Exception message parser for <see cref="OperationError.Message"/></param>
        public OperationResultExceptionJsonWrapperConfiguration SetExceptionWrapper<T>(int statusCode, int errorCode, Func<T, IApiErrorCodes, string> exceptionMsgParser)
            where T : Exception
        {
            var exParser = new Func<Exception, IApiErrorCodes, string>((ex, errorCodes) =>
            {
                return exceptionMsgParser((T)ex, errorCodes);
            });
            var cfg = new OperationErrorConfigurationEntry(errorCode, statusCode, exParser);
            SetExceptionWrapper(typeof(T), cfg);

            return this;
        }
    }
}
