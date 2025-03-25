namespace Taskd_manage_tags.src.models
{
    public class TaskTagList(List<TaskTag> taskTags) : ListResponse<TaskTag>(taskTags)
    {
    }
}