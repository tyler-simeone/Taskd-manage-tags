namespace manage_tags.src.models
{
    public class TagList : ResponseBase
    {
        public TagList()
        {
            Tags = new List<Tag>();
        }

        public List<Tag> Tags { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}