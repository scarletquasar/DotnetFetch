using DotnetFetch.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DotnetFetch
{
    public static class GlobalFetch
    {
        public static bool EnableCorsException { get; set; } = false;

        public static async Task<Response> Fetch(
            string resource,
            JsonObject? options = default,
            CancellationToken cancellationToken = default
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
                options?["credentials"]?.ToString() ?? "same-orgin",
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

            var client = new HttpClient { BaseAddress = new Uri(resource) };

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

            HttpMethod httpMethod = fetchOptions.Method.ToLower() switch
            {
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post,
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
#if NETSTANDARD
                "patch" => new HttpMethod("PATCH"),
#else
                "patch" => HttpMethod.Patch,
#endif
                "head" => HttpMethod.Head,
                "options" => HttpMethod.Options,
                _ => throw new FetchInvalidMethodException(fetchOptions.Method),
            };

            HttpRequestMessage requestMessage = new(httpMethod, resource) { Content = content };

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
