using Taskd_manage_tags.src.util;

namespace Taskd_manage_tags.src.models.errors
{
    public class ExistingTagError(
        int statusCode,
        string exceptionMessage
        ) : Error(statusCode, exceptionMessage, ErrorTypes.ExistingTagError)
    {
    }
}