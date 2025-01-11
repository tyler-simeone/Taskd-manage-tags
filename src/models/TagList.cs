namespace Taskd_manage_tags.src.models
{
    public class TagList(List<Tag> tags) : ListResponse<Tag>(tags)
    {
    }
}