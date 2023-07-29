using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DotnetFetch.Models
{
    /// <summary>
    /// Represents a response to a fetch request.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Creates a new <see cref="Response"/> object.
        /// </summary>
        /// <param name="body">The body of the response.</param>
        /// <param name="headers">The headers of the response.</param>
        /// <param name="status">The status code of the response.</param>
        /// <param name="statusText">The status text of the response.</param>
        /// <param name="ok">Whether or not the response was successful.</param>
        /// <param name="bodyUsed">Whether or not the body of the response has been used.</param>
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

        /// <summary>
        /// Gets or sets the body of the response.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the headers of the response.
        /// </summary>
        public Dictionary<string, dynamic> Headers { get; set; }

        /// <summary>
        /// Gets or sets the status code of the response.
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets the status text of the response.
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Gets or sets whether or not the response was successful.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// Gets or sets whether or not the body of the response has been used.
        /// </summary>
        public bool BodyUsed { get; set; }

        /// <summary>
        /// Deserializes the body of the response into a <see cref="JsonObject"/>.
        /// </summary>
        /// <returns>A <see cref="JsonObject"/> representing the body of the response.</returns>
        public JsonObject Json() => JsonSerializer.Deserialize<JsonObject>(Body)!;

        /// <summary>
        /// Gets the body of the response as text.
        /// </summary>
        /// <returns>A string representing the body of the response.</returns>
        public string Text() => Body;

        /// <summary>
        /// Gets the body of the response as a byte array.
        /// </summary>
        /// <returns>A byte array representing the body of the response.</returns>
        public byte[] Blob() => Encoding.UTF8.GetBytes(Text());

        /// <summary>
        /// Gets the body of the response as a byte array.
        /// </summary>
        /// <returns>A byte array representing the body of the response.</returns>
        public byte[] ArrayBuffer() => Blob();

        /// <summary>
        /// Clones the response.
        /// </summary>
        /// <returns>A new <see cref="Response"/> object with the same properties as the original.</returns>
        public Response Clone() => new(Body, Headers, Status, StatusText, Ok, BodyUsed);
    }
}
