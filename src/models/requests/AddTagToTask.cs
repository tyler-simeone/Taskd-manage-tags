namespace Taskd_manage_tags.src.models.requests
{
    public class AddTagToTask
    {
        public AddTagToTask()
        {

        }

        public int UserId { get; set; }

        public int BoardId { get; set; }

        public int TagId { get; set; }
        
        public int TaskId { get; set; }
    }
}