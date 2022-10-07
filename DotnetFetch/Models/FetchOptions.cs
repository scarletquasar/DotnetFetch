using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    public class FetchOptions
    {
        public FetchOptions(
            JsonNode headers,
            string body,
            string method,
            string mode,
            string credentials,
            string cache,
            string redirect,
            bool keepAlive
        )
        {
            Headers = headers;
            Body = body;
            Method = method;
            Mode = mode;
            Credentials = credentials;
            Cache = cache;
            Redirect = redirect;
            KeepAlive = keepAlive;
        }

        public JsonNode Headers { get; set; }
        public string Body { get; set; }
        public string Method { get; set; }
        public string Mode { get; set; }
        public string Credentials { get; set; }
        public string Cache { get; set; }
        public string Redirect { get; set; }
        public bool KeepAlive { get; set; }
    }
}
