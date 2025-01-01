using Taskd_manage_tags.src.models;

namespace Taskd_manage_tags.src.dataservice
{
    public interface ITagsDataservice
    {
        Task<TagList> GetTagsByBoardId(int userId, int boardId);

        Task<TaskTagList> GetTaskTagsByUserIdAndBoardId(int userId, int boardId);

        Task<int> CreateTag(string tagName, int userId, int boardId);

        Task<int> AddTagToTask(int userId, int boardId, int tagId, int taskId);
        
        void DeleteTag(int tagId, int userId);

        void DeleteTagFromTask(int taskTagId, int userId);
    }
}