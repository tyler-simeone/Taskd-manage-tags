namespace Taskd_manage_tags.src.models
{
    public class TagList : Response
    {
        public TagList()
        {
            Tags = [];
        }

        public List<Tag> Tags { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}