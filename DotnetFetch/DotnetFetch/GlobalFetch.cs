using DotnetFetch.Models;
using System.Text.Json.Nodes;

namespace DotnetFetch
{
    public static class GlobalFetch
    {
        public static async Task<Response> Fetch(string resource, JsonObject options)
        {
            var headers = 
                options.TryGetPropertyValue("headers", out var _headers) ? _headers : new JsonObject();

            var body =
                options.TryGetPropertyValue("body", out var _body) ? _body : new JsonObject();



            return new();
        }
    }
}