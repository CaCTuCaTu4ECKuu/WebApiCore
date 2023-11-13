using System;
using WebApiCore.Exceptions;

namespace WebApiCore.AspNetCore.Middleware.AccessTokenAuthentication
{
    public class AccessTokenException : Exception
    {
        /// <summary>
        /// Error code that indicates what kind of error happened
        /// </summary>
        public int ErrorCode { get; protected set; } = BasicApiErrorCodes.ACCESS_TOKEN_ERROR;

        public AccessTokenException(string message)
            : base(message)
        { }

        public AccessTokenException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public class AccessTokenExpiredException : AccessTokenException
    {
        public AccessTokenExpiredException(string message)
            : base(BasicApiErrorCodes.ACCESS_TOKEN_EXPIRED, message)
        { }

        public AccessTokenExpiredException(int errorCode, string message)
            : base(errorCode, message)
        { }
    }

    public class AccessTokenInvalidException : AccessTokenException
    {
        public AccessTokenInvalidException(string message)
            : base(BasicApiErrorCodes.ACCESS_TOKEN_INVALID, message)
        { }

        public AccessTokenInvalidException(int errorCode, string message)
            : base(errorCode, message)
        { }
    }
}
