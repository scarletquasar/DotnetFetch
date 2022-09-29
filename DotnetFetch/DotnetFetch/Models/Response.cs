using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    public class Response
    {
        public JsonObject Body { get; set; }
        public JsonObject Headers { get; set; }
        public short Status { get; set; } 
        public string StatusText { get; set; }
        public string ResponseStatus { get; set; }
        public bool Ok { get; set; }
        public bool BodyUsed { get; set; }
        public bool Redirected { get; set; }

        public JsonObject Json()
        {
            return new JsonObject();
        }

        public string Text()
        {
            return new("");
        }

        public byte[] Blob()
        {
            return default;
        }

        public byte[] ArrayBuffer()
        {
            return default;
        }

        public Response Clone()
        {
            return new();
        }

        public Response Redirect()
        {
            return new();
        }
    }
}
