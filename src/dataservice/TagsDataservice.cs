using System;
using System.Data;
using manage_tags.src.models;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace manage_tags.src.dataservice
{
    public class TagsDataservice : ITagsDataservice
    {
        private IConfiguration _configuration;
        private string _conx;

        public TagsDataservice(IConfiguration configuration)
        {
            _configuration = configuration;
            _conx = _configuration["LocalDBConnection"];

            if (_conx.IsNullOrEmpty())
                _conx = _configuration.GetConnectionString("LocalDBConnection");
        }

        public async Task<TagList> GetTags(int userId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.TagGetAllByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@paramUserId", userId);

                    try
                    {
                        await connection.OpenAsync();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            var tagList = new TagList();

                            while (reader.Read())
                            {
                                models.Tag tag = ExtractTagFromReader(reader);
                                tagList.Tags.Add(tag);
                            }

                            return tagList;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async void DeleteTag(int tagId, int userId)
        {
            using (MySqlConnection connection = new(_conx))
            {
                using (MySqlCommand command = new("taskd_db_dev.TagDelete", connection))
                {
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
            }
        }

        #region HELPERS

        private models.Tag ExtractTagFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32("TagId");
            string name = reader.GetString("TagName");
            DateTime createDatetime = reader.GetDateTime("CreateDatetime");
            int createUserId = reader.GetInt32("CreateUserId");
            DateTime updateDatetime = reader.GetDateTime("UpdateDatetime");
            int updateUserId = reader.GetInt32("UpdateUserId");

            return new models.Tag()
            {
                TagId = id,
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