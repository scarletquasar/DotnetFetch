using DotnetFetch.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DotnetFetch
{
    public static class GlobalFetch
    {
        public static async Task<Response> Fetch(
            string resource, 
            JsonObject options, 
            CancellationToken cancellationToken = default
        )
        {
            // Arrange: will get all the possible information from "options"
            // object, in order to make the request based in the user needs

            var headers = options["headers"] ?? new JsonObject();
            var body = options["body"]?.ToJsonString() ?? "";

            //get | post | delete | put | patch | head |options
            var method = options["method"]?.ToString() ?? "get";

            //cors | no-cors | same-origin
            var mode = options["mode"]?.ToString() ?? "no-cors";

            //omit | same-origin | include
            var credentials = options["credentials"]?.ToString() ?? "same-orgin";

            //default | no-store | reload | no-cache | force-cache | only-if-cached
            var cache = options["cache"]?.ToString() ?? "default";

            //follow | error | manual
            var redirect = options["redirect"]?.ToString() ?? "follow";

            //[origin url] | about&nbsp:client
            var referrer = options["referrer"]?.ToString() ?? "";

            var integrity = options["integrity"]?.ToString() ?? "";
            var keepAlive = (bool)(options["integrity"] ?? false);

            //TODO: implement [signal] usage as AbortSignal

            // Arrange: will mount an HttpClient object based in the resource
            // and options, passed as arguments by the user

            var client = new HttpClient
            {
                BaseAddress = new Uri(resource)
            };

            // Arrange: will mount and apply the headers dictionary with the headers
            // JsonObject (for now, dynamic as value type is the best approach here)

            var headersJson = headers.ToJsonString();
            var headersBytes = new MemoryStream(Encoding.UTF8.GetBytes(headersJson));
            var headersDictionary = await JsonSerializer.DeserializeAsync<Dictionary<string, dynamic>>(
                headersBytes, 
                cancellationToken: cancellationToken
            );

            headersDictionary?
                .ToList()
                .ForEach(
                    header => client.DefaultRequestHeaders.Add(header.Key, header.Value)
                 );

            // Arrange: will get the encoding (Accept-Charset) and mime type
            // (Content-Type) headers to be passed into the HttpClient request

            string acceptCharset = (headersDictionary ?? new()).TryGetValue(
                "Accept-Charset", out var _acceptCharset
            ) ? _acceptCharset : "utf-8"; 

            var charset = acceptCharset.ToLowerInvariant() switch
            {
                "utf-8" => Encoding.UTF8,
                "utf-32" => Encoding.UTF32,
                "unicode" => Encoding.Unicode,
                _ => throw new FetchInvalidCharsetException(acceptCharset),
            };

            string contentType = (headersDictionary ?? new()).TryGetValue(
                "Content-Type", out var _contentType
            ) ? _contentType : "text/plain";

            // Arrange: will generate the StringContent object containing the body,
            // charset and content type to be passed directly to the HttpClient request,
            // then, check the argument http method and start the request

            var content = new StringContent(body, charset, contentType);

            HttpResponseMessage result;

            switch(method.ToLower())
            {
                case "get":
                    result = await client.GetAsync(resource, cancellationToken);
                    break;

                case "post":
                    result = await client.PostAsync(resource, content, cancellationToken);
                    break;

                case "put":
                    result = await client.PutAsync(resource, content, cancellationToken);
                    break;

                case "delete":
                    result = await client.DeleteAsync(resource);
                    break;
            }

            return new();
        }
    }
}