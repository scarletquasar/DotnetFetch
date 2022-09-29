using DotnetFetch.Models;
using System.Text.Json.Nodes;

namespace DotnetFetch
{
    public static class GlobalFetch
    {
        public static async Task<Response> Fetch(string resource, JsonObject options)
        {
            var headers = options["headers"] ?? new JsonObject();
            var body = options["body"] ?? new JsonObject();
            var method = options["method"]?.ToString() ?? "get"; //get | post | delete | put | patch | head |options
            var mode = options["mode"]?.ToString() ?? "no-cors"; //cors | no-cors | same-origin
            var credentials = options["credentials"]?.ToString() ?? "same-orgin"; //omit | same-origin | include


            return new();
        }
    }
}