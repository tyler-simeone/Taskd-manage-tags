using manage_tags.src.models;

namespace manage_tags.src.dataservice
{
    public interface ITagsDataservice
    {
        Task<TagList> GetTags(int userId);

        void DeleteTag(int tagId, int userId);
    }
}