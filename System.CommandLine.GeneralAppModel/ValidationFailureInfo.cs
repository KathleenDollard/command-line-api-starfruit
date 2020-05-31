namespace System.CommandLine.GeneralAppModel
{
    public class ValidationFailureInfo
    {
        public ValidationFailureInfo(string id, string path, string message)
        {
            Id = id;
            Path = path;
            Message = message;
        }

        public string Id { get; }
        public string Path { get; }
        public string Message { get; }
    }
}