using Taskd_manage_tags.src.models.errors;

namespace Taskd_manage_tags.src.util
{
    public class ErrorHelper
    {
        public static object MapExceptionToError(Error ex)
        {
            return new
            {
                ex.StatusCode,
                ex.ErrorType,
                ex.ExceptionDetails
            };
        }
    }
}