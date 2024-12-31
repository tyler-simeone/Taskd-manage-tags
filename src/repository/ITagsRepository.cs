using Taskd_manage_tags.src.models;

namespace Taskd_manage_tags.src.repository
{
    public interface ITagsRepository
    {
        Task<TagList> GetTags(int userId, int boardId);

        Task<int> CreateTag(string tagName, int userId, int boardId);
        
        Task<int> AddTagToTask(int userId, int boardId, int tagId, int taskId);

        void DeleteTag(int taskId, int userId);
    }
}