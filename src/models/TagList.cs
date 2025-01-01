namespace Taskd_manage_tags.src.models
{
    public class TagList : ListResponse
    {
        public TagList()
        {
            Data = [];
        }
        
        public List<Tag> Data { get; set; }
    }
}