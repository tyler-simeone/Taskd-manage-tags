using Taskd_manage_tags.src.dataservice;
using Taskd_manage_tags.src.models;

namespace Taskd_manage_tags.src.repository
{
    public class TagsRepository(ITagsDataservice tagsDataservice) : ITagsRepository
    {
        readonly ITagsDataservice _tagsDataservice = tagsDataservice;

        /// <summary>
        /// Get all tags per board. The list of available tags to add to a task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TagList> GetTagsByBoardId(int userId, int boardId)
        {
            try
            {
                TagList tagList = await _tagsDataservice.GetTagsByBoardId(userId, boardId);
                return tagList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Get all tags by task ID. Will filter out any tags that have already been assigned to the Task.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TagList> GetAvailableTagsByTaskIdAndBoardId(int taskId, int boardId)
        {
            try
            {
                TagList tagList = await _tagsDataservice.GetAvailableTagsByTaskIdAndBoardId(taskId, boardId);
                return tagList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TaskTagList> GetTaskTagsByTaskIdAndBoardId(int taskId, int boardId)
        {
            try
            {
                var taskTags = await _tagsDataservice.GetTaskTagsByTaskIdAndBoardId(taskId, boardId);
                return new TaskTagList(taskTags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all tags with their parent tasks. Board-level view of all tags on their Tasks.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TaskTagList> GetTaskTagsByUserIdAndBoardId(int userId, int boardId)
        {
            try
            {
                TaskTagList taskTagList = await _tagsDataservice.GetTaskTagsByUserIdAndBoardId(userId, boardId);
                return taskTagList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateTag(string tagName, int userId, int boardId)
        {
            try
            {
                return await _tagsDataservice.CreateTag(tagName, userId, boardId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        
        public async Task<int> AddTagToTask(int userId, int boardId, int tagId, int taskId)
        {
            try
            {
                return await _tagsDataservice.AddTagToTask(userId, boardId, tagId, taskId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public void DeleteTag(int tagId, int userId)
        {
            try
            {
                _tagsDataservice.DeleteTag(tagId, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        
        public void DeleteTagFromTask(int taskTagId, int userId)
        {
            try
            {
                _tagsDataservice.DeleteTagFromTask(taskTagId, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}