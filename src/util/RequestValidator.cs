public interface IRequestValidator
{
    bool ValidateGetTags(int userId);
    
    bool ValidateCreateTag(CreateTag tag);

    bool ValidateDeleteTag(int tagId, int userId);
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

    public bool ValidateCreateTag(CreateTag tag)
    {
        return true;
    }
    
    public bool ValidateDeleteTag(int tagId, int userId)
    {
        return true;
    }
}