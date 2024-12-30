using Taskd_manage_tags.src.models;

namespace Taskd_manage_tags.src.repository
{
    public interface ITagsRepository
    {
        Task<TagList> GetTags(int userId, int boardId);

        Task<int> CreateTag(string tagName, int userId, int boardId);

        void DeleteTag(int taskId, int userId);
    }
}