namespace Taskd_manage_tags.src.models
{
    public class ListResponse : Response
    {
        public ListResponse()
        {

        }

        public int Total { get; set; }
        
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}