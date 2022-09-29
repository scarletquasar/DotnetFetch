namespace DotnetFetch.Models
{
    public class FetchInvalidCharsetException : Exception
    {
        public FetchInvalidCharsetException(string charset) : base($"Invalid charset: {charset}") { }
    }
}
