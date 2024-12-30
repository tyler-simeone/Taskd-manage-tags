namespace Taskd_manage_tags.src.models.requests
{
    public class CreateTag
    {
        public CreateTag()
        {

        }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public string TagName { get; set; }
    }
}