using Taskd_manage_tags.src.util;

namespace Taskd_manage_tags.src.models.errors
{
    public class ExistingTaskTagError(
        int statusCode,
        string exceptionMessage
        ) : Error(statusCode, exceptionMessage, ErrorTypes.ExistingTaskTagError)
    {
    }
}