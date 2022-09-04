namespace WebApiCore.Exceptions
{
    public interface IApiErrorCodes
    {
        /// <summary>
        /// Indicates if application error code is defined
        /// </summary>
        /// <param name="errorCode">Application error code</param>
        bool HasCode(int errorCode);

        /// <summary>
        /// Get HTTP repsonse code corresponding to application error code
        /// </summary>
        /// <param name="errorCode">Application error code</param>
        int GetHTTPResponseCode(int errorCode);

        /// <summary>
        /// Get default error message corresponding to application error code
        /// </summary>
        /// <param name="errorCode">Application error code</param>
        string GetErrorMessage(int errorCode);

        /// <summary>
        /// Get default error message for application
        /// </summary>
        string GetUnknownErrorMessage();
    }
}
