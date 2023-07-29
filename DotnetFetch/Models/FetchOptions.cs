using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    /// <summary>
    /// Represents the options to use when fetching a resource from an external provider over the network.
    /// </summary>
    public class FetchOptions
    {
        /// <summary>
        /// Creates a new <see cref="FetchOptions"/> object.
        /// </summary>
        /// <param name="headers">The headers to use when fetching the resource.</param>
        /// <param name="body">The body to use when fetching the resource.</param>
        /// <param name="method">The method to use when fetching the resource.</param>
        /// <param name="mode">The mode to use when fetching the resource.</param>
        /// <param name="credentials">The credentials to use when fetching the resource.</param>
        /// <param name="cache">The cache to use when fetching the resource.</param>
        /// <param name="redirect">The redirect to use when fetching the resource.</param>
        /// <param name="keepAlive">Whether or not to keep the connection alive when fetching the resource.</param>
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

        /// <summary>
        /// Gets or sets the headers to use when fetching the resource.
        /// </summary>
        public JsonNode Headers { get; set; }

        /// <summary>
        /// Gets or sets the body to use when fetching the resource.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the method to use when fetching the resource.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the mode to use when fetching the resource.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets the credentials to use when fetching the resource.
        /// </summary>
        public string Credentials { get; set; }

        /// <summary>
        /// Gets or sets the cache to use when fetching the resource.
        /// </summary>
        public string Cache { get; set; }

        /// <summary>
        /// Gets or sets the redirect to use when fetching the resource.
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// Gets or sets whether or not to keep the connection alive when fetching the resource.
        /// </summary>
        public bool KeepAlive { get; set; }
    }
}
