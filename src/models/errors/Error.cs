namespace Taskd_manage_tags.src.models.errors
{
    public class Error(
        int statusCode,
        string exceptionMessage,
        string errorType
        ) : Exception
    {
        public int StatusCode { get; set; } = statusCode;

        public string ExceptionMessage { get; set; } = exceptionMessage;

        public string ErrorType { get; set; } = errorType;
    }
}