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
        public static async Task<Response> Fetch(
            string resource,
            JsonObject? options = default,
            CancellationToken cancellationToken = default
        )
        {
            // Arrange: will get all the possible information from "options"
            // object, in order to make the request based in the user needs

            var headers = options?["headers"] ?? new JsonObject();
            var body = options?["body"]?.ToJsonString() ?? "";

            //get | post | delete | put | patch | head |options
            var method = options?["method"]?.ToString() ?? "get";

            //cors | no-cors | same-origin
            var mode = options?["mode"]?.ToString() ?? "no-cors";

            //omit | same-origin | include
            var credentials = options?["credentials"]?.ToString() ?? "same-orgin";

            //default | no-store | reload | no-cache | force-cache | only-if-cached
            var cache = options?["cache"]?.ToString() ?? "default";

            //follow | manual // no support: error
            var redirect = options?["redirect"]?.ToString() ?? "follow";

            var keepAlive = (bool)(options?["keep-alive"] ?? false);

            // Arrange: will mount an HttpClient object based in the resource
            // and options, passed as arguments by the user, including handler 
            // configuration

            HttpClientHandler httpHandler = new()
            {
                AllowAutoRedirect = redirect == "follow"
            };

            var client = new HttpClient { BaseAddress = new Uri(resource) };

            // Arrange: will mount and apply the headers dictionary with the headers
            // JsonObject (for now, dynamic as value type is the best approach here)

            var headersJson = headers.ToJsonString();
            var headersBytes = new MemoryStream(Encoding.UTF8.GetBytes(headersJson));
            var headersDictionary = await JsonSerializer.DeserializeAsync<
                Dictionary<string, dynamic>
            >(headersBytes, cancellationToken: cancellationToken);

            headersDictionary
                ?.ToList()
                .ForEach(header => client.DefaultRequestHeaders.Add(header.Key, header.Value));

            // Arrange: will get the mode (cors) option to be passed as a
            // (Sec-Fetch-Mode) header

            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", mode);

            // Arrange: will get the credentials option to be passed as a
            // (Access-Control-Allow-Credentials) header

            var credentialsHeader = (!(credentials == "omit")).ToString().ToLower();
            client.DefaultRequestHeaders.Add("Access-Control-Allow-Credentials", credentialsHeader);

            // Arrange: will get the keep-alive option to be passed as a
            // (Connection-Close) header

            client.DefaultRequestHeaders.ConnectionClose = keepAlive;

            // Arrange: will get the cache option to be passed as a
            // (Cache-Control) header

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = cache == "default" || cache == "force-cache"
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

            var content = new StringContent(body, charset, contentType);

            HttpMethod httpMethod = method.ToLower() switch
            {
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post,
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
                "patch" => HttpMethod.Patch,
                "head" => HttpMethod.Head,
                "options" => HttpMethod.Options,
                _ => throw new FetchInvalidMethodException(method),
            };

            HttpRequestMessage requestMessage = new(httpMethod, resource)
            {
                Content = content
            };

            var result = await client.SendAsync(requestMessage, cancellationToken);

            var resultBody = await result.Content.ReadAsStringAsync(cancellationToken);
            var resultHeaders = result.Headers.ToDictionary(x => x.Key, x => (dynamic)x.Value);
            var status = (short)result.StatusCode;
            var statusText = ReasonPhrases.GetReasonPhrase(status);
            var ok = result.IsSuccessStatusCode;
            var bodyUsed = body != "" && method != "get" && method != "delete";

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
