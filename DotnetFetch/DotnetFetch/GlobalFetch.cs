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

            return new();
        }
    }
}