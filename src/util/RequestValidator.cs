public interface IRequestValidator
{
    bool ValidateGetTags(int userId);
    
    bool ValidateCreateTag(CreateTag createTaskRequest);

    bool ValidateDeleteTask(int tagId, int userId);
}

public class RequestValidator : IRequestValidator
{
    public RequestValidator()
    {
        
    }

    public bool ValidateGetTags(int userId)
    {
        return true;
    }

    public bool ValidateCreateTag(CreateTag createTaskRequest)
    {
        return true;
    }
    
    public bool ValidateDeleteTask(int tagId, int userId)
    {
        return true;
    }
}