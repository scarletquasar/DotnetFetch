using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    public class Response
    {
        public Response(
            string body,
            Dictionary<string, dynamic> headers,
            short status,
            string statusText,
            bool ok,
            bool bodyUsed
        )
        {
            Body = body;
            Headers = headers;
            Status = status;
            StatusText = statusText;
            Ok = ok;
            BodyUsed = bodyUsed;
        }

        public string Body { get; set; }
        public Dictionary<string, dynamic> Headers { get; set; }
        public short Status { get; set; }
        public string StatusText { get; set; }
        public bool Ok { get; set; }
        public bool BodyUsed { get; set; }

        public JsonObject Json() => JsonSerializer.Deserialize<JsonObject>(Body)!;

        public string Text() => Body;

        public byte[] Blob() => Encoding.UTF8.GetBytes(Text());

        public byte[] ArrayBuffer() => Blob();

        public Response Clone() =>
            new(
                Body,
                Headers,
                Status,
                StatusText,
                Ok,
                BodyUsed
            );
    }
}
