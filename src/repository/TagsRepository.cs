using manage_tags.src.dataservice;
using manage_tags.src.models;

namespace manage_tags.src.repository
{
    public class TagsRepository : ITagsRepository
    {
        ITagsDataservice _tagsDataservice;

        public TagsRepository(ITagsDataservice tagsDataservice)
        {
            _tagsDataservice = tagsDataservice;
        }

        public async Task<TagList> GetTags(int userId, int boardId)
        {
            try
            {
                TagList tagList = await _tagsDataservice.GetTags(userId, boardId);
                return tagList;
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
    }
}