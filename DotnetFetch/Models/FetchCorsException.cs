namespace DotnetFetch.Models
{
    public class FetchCorsException : ArgumentException
    {
        public FetchCorsException() : 
            base("A CORS excepion has occurred, check your request options") { }
    }
}
