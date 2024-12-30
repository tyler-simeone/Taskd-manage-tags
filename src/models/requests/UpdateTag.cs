namespace Taskd_manage_tags.src.models.requests
{
    public class UpdateTag
    {
        public UpdateTag()
        {

        }

        public int UserId { get; set; }

        public int TagId { get; set; }

        public string TagName { get; set; }
    }
}