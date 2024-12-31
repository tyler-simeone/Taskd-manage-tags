namespace Taskd_manage_tags.src.util
{
    public static class ErrorTypes
    {
        public const string ExistingTagError = "ExistingTagError";
        
        public const string ExistingTaskTagError = "ExistingTaskTagError";
    }
    
    public static class ErrorMessages
    {
        public const string ExistingTagError = "A tag with the provided name already exists for board ID: {0}.";
        
        public const string ExistingTaskTagError = "The task provided already has the provided tag ID: {0}.";
    }
}