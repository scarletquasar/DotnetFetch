using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    public class Response
    {
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
    }
}
