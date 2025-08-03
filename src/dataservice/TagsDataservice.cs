using System.Data;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Taskd_manage_tags.src.models;
using Taskd_manage_tags.src.models.errors;
using Taskd_manage_tags.src.util;

namespace Taskd_manage_tags.src.dataservice
{
    public class TagsDataservice : ITagsDataservice
    {
        private readonly IConfiguration _configuration;
        private readonly string _conx;

        public TagsDataservice(IConfiguration configuration)
        {
            _configuration = configuration;
            _conx = _configuration["LocalDBConnection"];

            if (_conx.IsNullOrEmpty())
                _conx = _configuration.GetConnectionString("LocalDBConnection");
        }

        /// <summary>
        /// Get all tags per board. The list of available tags to add to a task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TagList> GetTagsByBoardId(int userId, int boardId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TagGetAllByUserIdAndBoardId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramUserId", userId);
            command.Parameters.AddWithValue("@paramBoardId", boardId);

            var tags = new List<Tag>();

            try
            {
                await connection.OpenAsync();
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tag tag = ExtractTagFromReader(reader);
                    tags.Add(tag);
                }

                return new TagList(tags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTagsByBoardId Error: {ex.Message}");
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
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TagGetAvailableListByTaskIdAndBoardId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramTaskId", taskId);
            command.Parameters.AddWithValue("@paramBoardId", boardId);

            var tags = new List<Tag>();

            try
            {
                await connection.OpenAsync();
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tag tag = ExtractTagFromReader(reader);
                    tags.Add(tag);
                }

                return new TagList(tags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTaskTagsByTaskIdAndBoardId Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all tags assigned to an existing task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<List<TaskTag>> GetTaskTagsByTaskIdAndBoardId(int taskId, int boardId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TaskTagGetListByTaskIdAndBoardId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramTaskId", taskId);
            command.Parameters.AddWithValue("@paramBoardId", boardId);

            var taskTags = new List<TaskTag>();

            try
            {
                await connection.OpenAsync();
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TaskTag taskTag = ExtractTaskTagFromReader(reader);
                    taskTags.Add(taskTag);
                }

                return taskTags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTagsByTaskId Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all tags with their parent tasks. Board-level view of all tags on their Tasks.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<TaskTagList> GetAssignedTagsByUserIdAndBoardId(int userId, int boardId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TaskTagGetListByUserIdAndBoardId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramUserId", userId);
            command.Parameters.AddWithValue("@paramBoardId", boardId);

            var taskTags = new List<TaskTag>();

            try
            {
                await connection.OpenAsync();
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TaskTag taskTag = ExtractTaskTagFromReader(reader);
                    taskTags.Add(taskTag);
                }

                return new TaskTagList(taskTags);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTagsByTaskId Error: {ex.Message}");
                throw;
            }
        }

        public async Task<int> CreateTag(string tagName, int userId, int boardId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using MySqlCommand command = new("taskd_db_dev.TagCheckIsUnique", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paramTagName", tagName);
                command.Parameters.AddWithValue("@paramBoardId", boardId);

                try
                {
                    await connection.OpenAsync();

                    using MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                            throw new ExistingTagError(
                                StatusCodes.Status500InternalServerError,
                                String.Format(ErrorMessages.ExistingTagError, boardId)
                            );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TagCheckIsUnique Error: {ex.Message}");
                    throw;
                }
            }

            using (MySqlConnection connection = new(_conx))
            {
                using MySqlCommand command = new("taskd_db_dev.TagPersist", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paramTagName", tagName);
                command.Parameters.AddWithValue("@paramBoardId", boardId);
                command.Parameters.AddWithValue("@paramCreateUserId", userId);

                int tagId = 0;

                try
                {
                    await connection.OpenAsync();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tagId = reader.GetInt32("TagId");
                        }
                    }

                    return tagId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TagPersist Error: {ex.Message}");
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Connects a tag to a task.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="boardId"></param>
        /// <param name="tagId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        /// <exception cref="ExistingTaskTagError"></exception>
        public async Task<int> AddTagToTask(int userId, int boardId, int tagId, int taskId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using MySqlCommand command = new("taskd_db_dev.TaskTagCheckIsUnique", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paramTagId", tagId);
                command.Parameters.AddWithValue("@paramTaskId", taskId);

                try
                {
                    await connection.OpenAsync();

                    using MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("TaskTagId")))
                        {
                            throw new ExistingTaskTagError(StatusCodes.Status500InternalServerError,
                                                           String.Format(ErrorMessages.ExistingTaskTagError, taskId, tagId));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TagTaskCheckIsUnique Error: {ex.Message}");
                    throw;
                }
            }

            using (MySqlConnection connection = new(_conx))
            {
                using MySqlCommand command = new("taskd_db_dev.TagTaskPersist", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@paramTagId", tagId);
                command.Parameters.AddWithValue("@paramTaskId", taskId);
                command.Parameters.AddWithValue("@paramCreateUserId", userId);

                int taskTagId = 0;

                try
                {
                    await connection.OpenAsync();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskTagId = reader.GetInt32("TaskTagId");
                        }
                    }

                    return taskTagId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TaskTagPersist Error: {ex.Message}");
                    throw;
                }
            }
        }

        public async void DeleteTag(int tagId, int userId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TagDelete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramTagId", tagId);
            command.Parameters.AddWithValue("@paramUpdateUserId", userId);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteTag Error: {ex.Message}");
                throw;
            }
        }
        
        public async void DeleteTagFromTask(int taskTagId, int userId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TagTaskDelete", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramTaskTagId", taskTagId);
            command.Parameters.AddWithValue("@paramUpdateUserId", userId);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteTag Error: {ex.Message}");
                throw;
            }
        }

        #region HELPERS

        private static Tag ExtractTagFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32("TagId");
            int boardId = reader.GetInt32("BoardId");
            string name = reader.GetString("TagName");
            DateTime createDatetime = reader.GetDateTime("CreateDatetime");
            int createUserId = reader.GetInt32("CreateUserId");
            DateTime? updateDatetime = reader.IsDBNull(reader.GetOrdinal("UpdateDatetime")) ? null : reader.GetDateTime("UpdateDatetime");
            int? updateUserId = reader.IsDBNull(reader.GetOrdinal("UpdateUserId")) ? null : reader.GetInt32("UpdateUserId");

            return new Tag
            {
                TagId = id,
                BoardId = boardId,
                TagName = name,
                CreateDatetime = createDatetime,
                CreateUserId = createUserId,
                UpdateDatetime = updateDatetime,
                UpdateUserId = updateUserId
            };
        }
        
        private static TaskTag ExtractTaskTagFromReader(MySqlDataReader reader)
        {
            int tagId = reader.GetInt32("TagId");
            int boardId = reader.GetInt32("BoardId");
            int taskId = default;
            int taskTagId = default;
            try
            {
                taskId = reader.IsDBNull(reader.GetOrdinal("TaskId")) ? default : reader.GetInt32("TaskId");
                taskTagId = reader.IsDBNull(reader.GetOrdinal("TaskTagId")) ? default : reader.GetInt32("TaskTagId");
            }
            catch
            { 
                //ignore and move on
            }
            string name = reader.GetString("TagName");
            DateTime createDatetime = reader.GetDateTime("CreateDatetime");
            int createUserId = reader.GetInt32("CreateUserId");
            DateTime? updateDatetime = reader.IsDBNull(reader.GetOrdinal("UpdateDatetime")) ? null : reader.GetDateTime("UpdateDatetime");
            int? updateUserId = reader.IsDBNull(reader.GetOrdinal("UpdateUserId")) ? null : reader.GetInt32("UpdateUserId");

            return new TaskTag
            {
                TaskTagId = taskTagId,
                TagId = tagId,
                TaskId = taskId,
                BoardId = boardId,
                TagName = name,
                CreateDatetime = createDatetime,
                CreateUserId = createUserId,
                UpdateDatetime = updateDatetime,
                UpdateUserId = updateUserId
            };
        }

        #endregion
    }
}