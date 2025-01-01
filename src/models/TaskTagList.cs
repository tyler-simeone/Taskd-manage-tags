namespace Taskd_manage_tags.src.models
{
    public class TaskTagList : ListResponse
    {
        public TaskTagList()
        {
            Data = [];
        }
        
        public List<TaskTag> Data { get; set; }
    }
}