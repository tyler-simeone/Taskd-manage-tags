using manage_tags.src.models;

namespace manage_tags.src.repository
{
    public interface ITagsRepository
    {
        Task<TagList> GetTags(int userId);

        void DeleteTag(int taskId, int userId);
    }
}