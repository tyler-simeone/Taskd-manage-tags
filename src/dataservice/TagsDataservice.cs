using System.Data;
using Taskd_manage_tags.src.models;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
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

        public async Task<TagList> GetTags(int userId, int boardId)
        {
            using MySqlConnection connection = new(_conx);
            using MySqlCommand command = new("taskd_db_dev.TagGetAllByUserIdAndBoardId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@paramUserId", userId);
            command.Parameters.AddWithValue("@paramBoardId", boardId);

            var tagList = new TagList();

            try
            {
                await connection.OpenAsync();
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tag tag = ExtractTagFromReader(reader);
                    tagList.Tags.Add(tag);
                }

                return tagList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTags Error: {ex.Message}");
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
                    Console.WriteLine($"taskd_db_dev.TagCheckIsUnique Error: {ex.Message}");
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
                    Console.WriteLine($"taskd_db_dev.TagPersist Error: {ex.Message}");
                    throw;
                }
            }
        }
        
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
                            throw new ExistingTaskTagError(
                                StatusCodes.Status500InternalServerError,
                                String.Format(ErrorMessages.ExistingTaskTagError, tagId)
                            );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"taskd_db_dev.TagCheckIsUnique Error: {ex.Message}");
                    throw;
                }
            }

            using (MySqlConnection connection = new(_conx))
            {
                using MySqlCommand command = new("taskd_db_dev.TaskTagPersist", connection);
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
                            tagId = reader.GetInt32("TaskTagId");
                        }
                    }

                    return taskTagId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"taskd_db_dev.TaskTagPersist Error: {ex.Message}");
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
                Console.WriteLine($"Error: {ex.Message}");
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

            return new Tag()
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

        #endregion
    }
}