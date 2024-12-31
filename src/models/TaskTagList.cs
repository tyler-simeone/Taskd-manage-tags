namespace Taskd_manage_tags.src.models
{
    public class TaskTagList : ListResponse
    {
        public TaskTagList()
        {
            TaskTags = [];
        }
        
        public List<TaskTag> TaskTags { get; set; }
    }
}