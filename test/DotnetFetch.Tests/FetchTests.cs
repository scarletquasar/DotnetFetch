using static DotnetFetch.GlobalFetch;
using DotnetFetch.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DotnetFetch.Tests
{
    public class FetchTests
    {
        [Fact]
        public async void FetchBasicFunctionShouldWorkProperly()
        {
            var result = await Fetch("https://jsonplaceholder.typicode.com/todos/1");
            var model = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result.Text())!;

            Assert.Equal(1, model["userId"].GetInt32());
            Assert.Equal(1, model["id"].GetInt32());
            Assert.Equal("delectus aut autem", model["title"].GetString());
            Assert.False(model["completed"].GetBoolean());
        }

        [Fact]
        public async void FetchGenericFunctionShouldWorkProperly()
        {
            var result = await Fetch<Dictionary<string, JsonElement>>(
                "https://jsonplaceholder.typicode.com/todos/1"
            );

            Assert.Equal(1, result["userId"].GetInt32());
            Assert.Equal(1, result["id"].GetInt32());
            Assert.Equal("delectus aut autem", result["title"].GetString());
            Assert.False(result["completed"].GetBoolean());
        }

        [Fact]
        public async Task FetchInvalidCharsetShouldThrowException()
        {
            var fetchOptions = new JsonObject
            {
                ["headers"] = new JsonObject() { ["Accept-Charset"] = "invalid" }
            };

            await Assert.ThrowsAsync<FetchInvalidCharsetException>(
                () => Fetch("https://jsonplaceholder.typicode.com/todos/1", fetchOptions)
            );
        }
    }
}
