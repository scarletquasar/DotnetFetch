namespace DotnetFetch.Models
{
    public class FetchInvalidMethodException : ArgumentException
    {
        public FetchInvalidMethodException(string method) : base($"Invalid method: {method}") { }
    }
}
