namespace manage_tags.src.models
{
    public class Tag : ResponseBase
    {
        public Tag()
        {

        }

        public int TagId { get; set; }
        
        public string TagName { get; set; }

        public DateTime CreateDatetime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime UpdateDatetime { get; set; }

        public int UpdateUserId { get; set; }
    }
}