using Taskd_manage_tags.src.util;

namespace Taskd_manage_tags.src.models.errors
{
    public class ExistingTagError(
        int statusCode,
        string exceptionMessage,
        int userId,
        int boardId,
        string tagName
        ) : Error(statusCode, exceptionMessage, ErrorTypes.ExistingTagError)
    {
        public int UserId { get; set; } = userId;

        public int BoardId { get; set; } = boardId;

        public string TagName { get; set; } = tagName;
    }
}