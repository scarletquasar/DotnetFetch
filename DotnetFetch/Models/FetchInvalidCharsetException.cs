namespace DotnetFetch.Models
{
    public class FetchInvalidCharsetException : ArgumentException
    {
        public FetchInvalidCharsetException(string charset) : base($"Invalid charset: {charset}")
        { }
    }
}
