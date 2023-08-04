using DotnetFetch.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace DotnetFetch
{
    /// <summary>
    /// Defines methods for fetching resources from an external provider over the network.
    /// </summary>
    public static class GlobalFetch
    {
        /// <summary>
        /// Gets or sets whether or not the fetch method will throw a <see cref="FetchCorsException"/> when a CORS error is detected.
        /// </summary>
        /// <remarks>The default value is <see langword="false"/>.</remarks>
        public static bool EnableCorsException { get; set; } = false;
        private static HttpClient? _internalSingletonClient = null;

        /// <summary>
        /// Fetches a resource from an external provider over the network.
        /// </summary>
        /// <param name="resource">The URL of the resource to fetch.</param>
        /// <param name="options">The options to use when fetching the resource.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
        /// <param name="injectableClient">An <see cref="HttpClient"/> to use for the request. If <see langword="null"/>, a new <see cref="HttpClient"/> will be created.</param>
        /// <returns>A <see cref="Response"/> object containing the response from the request.</returns>
        /// <exception cref="FetchCorsException">Thrown when a CORS error is detected and <see cref="EnableCorsException"/> is <see langword="true"/>.</exception>
        /// <exception cref="FetchInvalidCharsetException">Thrown when an invalid charset is detected.</exception>
        public static async Task<Response> Fetch(
            string resource,
            JsonObject? options = default,
            CancellationToken cancellationToken = default,
            HttpClient? injectableClient = default
        )
        {
            // Arrange: generating the FetchOptions object

            var fetchOptions = new FetchOptions(
                options?["headers"] ?? new JsonObject(),
                options?["body"]?.ToJsonString() ?? "",
                //get | post | delete | put | patch | head |options
                options?["method"]?.ToString() ?? "get",
                //cors | no-cors | same-origin
                options?["mode"]?.ToString() ?? "no-cors",
                //omit | same-origin | include
                options?["credentials"]?.ToString() ?? "same-origin",
                //default | no-store | reload | no-cache | force-cache | only-if-cached
                options?["cache"]?.ToString() ?? "default",
                //follow | manual // no support: error
                options?["redirect"]?.ToString() ?? "follow",
                (bool)(options?["keep-alive"] ?? false)
            );

            // Arrange: will mount an HttpClient object based in the resource
            // and options, passed as arguments by the user, including handler
            // configuration

            HttpClientHandler httpHandler =
                new() { AllowAutoRedirect = fetchOptions.Redirect == "follow" };

            HttpClient client;

            if (injectableClient != null)
            {
                client = injectableClient;

                if (injectableClient.BaseAddress is null)
                {
                    client.BaseAddress = new Uri(resource);
                }
            }
            else
            {
                _internalSingletonClient = new HttpClient();
                client = _internalSingletonClient;
                client.BaseAddress = new Uri(resource);
            }

            // Arrange: will mount and apply the headers dictionary with the headers
            // JsonObject (for now, dynamic as value type is the best approach here)

            var headersJson = fetchOptions.Headers.ToJsonString();
            var headersBytes = new MemoryStream(Encoding.UTF8.GetBytes(headersJson));
            var headersDictionary = await JsonSerializer.DeserializeAsync<
                Dictionary<string, string>
            >(headersBytes, cancellationToken: cancellationToken);

            headersDictionary
                ?.ToList()
                .ForEach(header => client.DefaultRequestHeaders.Add(header.Key, header.Value));

            // Arrange: will get the mode (cors) option to be passed as a
            // (Sec-Fetch-Mode) header

            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", fetchOptions.Mode);

            // Arrange: will get the credentials option to be passed as a
            // (Access-Control-Allow-Credentials) header

            var credentialsHeader = (!(fetchOptions.Credentials == "omit")).ToString().ToLower();
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Credentials", credentialsHeader);

            // Arrange: will get the keep-alive option to be passed as a
            // (Connection-Close) header

            client.DefaultRequestHeaders.ConnectionClose = fetchOptions.KeepAlive;

            // Arrange: will get the cache option to be passed as a
            // (Cache-Control) header

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = fetchOptions.Cache == "default" || fetchOptions.Cache == "force-cache"
            };

            // Arrange: will get the encoding (Accept-Charset) and mime type
            // (Content-Type) headers to be passed into the HttpClient request

            string acceptCharset = (headersDictionary ?? new()).TryGetValue(
                "Accept-Charset",
                out var _acceptCharset
            )
                ? _acceptCharset
                : "utf-8";

            var charset = acceptCharset.ToLowerInvariant() switch
            {
                "utf-8" => Encoding.UTF8,
                "utf-32" => Encoding.UTF32,
                "unicode" => Encoding.Unicode,
                _ => throw new FetchInvalidCharsetException(acceptCharset),
            };

            string contentType = (headersDictionary ?? new()).TryGetValue(
                "Content-Type",
                out var _contentType
            )
                ? _contentType
                : "text/plain";

            // Arrange: will generate the StringContent object containing the body,
            // charset and content type to be passed directly to the HttpClient request,
            // then, check the argument http method and start the request

            var content = new StringContent(fetchOptions.Body, charset, contentType);

            var httpMethod = new HttpMethod(fetchOptions.Method.ToUpper());
            var requestMessage = new HttpRequestMessage(httpMethod, resource) { Content = content };

            var result = await client.SendAsync(requestMessage, cancellationToken);
#if NETSTANDARD
            var resultBody = await result.Content.ReadAsStringAsync();
#else
            var resultBody = await result.Content.ReadAsStringAsync(cancellationToken);
#endif
            var resultHeaders = result.Headers.ToDictionary(x => x.Key, x => (dynamic)x.Value);
            var status = (short)result.StatusCode;
            var statusText = ReasonPhrases.GetReasonPhrase(status);
            var ok = result.IsSuccessStatusCode;
            var bodyUsed =
                fetchOptions.Body != ""
                && fetchOptions.Method != "get"
                && fetchOptions.Method != "delete";

            // Act: Checks for cors exceptions, surely, there is a better way
            // to check for CORS exceptions, a more complex research will be
            // made to create a more reliable solution

            if (
                EnableCorsException && !ok
                || (
                    resultBody.ToLower().Contains("access-control")
                    || resultBody.ToLower().Contains("cors")
                )
            )
            {
                throw new FetchCorsException();
            }

            // Act: Creates a new Response object, based on "fetch" default
            // specification with base on MDN

            return new(resultBody, resultHeaders, status, statusText, ok, bodyUsed);
        }

        /// <summary>
        /// Fetches a resource from an external provider over the network.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response body to.</typeparam>
        /// <param name="resource">The URL of the resource to fetch.</param>
        /// <param name="options">The options to use when fetching the resource.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>The deserialized response from the external provider.</returns>
        /// <remarks>This method is a shorthand for <see cref="Fetch(string, JsonObject?, CancellationToken, HttpClient?)"/> with <paramref name="injectableClient"/> set to <see langword="null"/>.</remarks>
        /// <seealso cref="Fetch(string, JsonObject?, CancellationToken, HttpClient?)"/>
        public static async Task<T> Fetch<T>(
            string resource,
            JsonObject? options = default,
            CancellationToken cancellationToken = default
        )
        {
            var response = await Fetch(resource, options, cancellationToken);
            var result = JsonSerializer.Deserialize<T>(response.Body);

            return result!;
        }
    }
}
