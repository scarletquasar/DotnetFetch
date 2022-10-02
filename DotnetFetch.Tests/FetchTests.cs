using DotnetFetch;
using System.Text.Json;

namespace DotnetFetch.Tests
{
    public class FetchTests
    {
        [Fact]
        public async void FetchBasicFunctionShouldWorkProperly()
        {
            var result = await GlobalFetch.Fetch("https://jsonplaceholder.typicode.com/todos/1");
            var model = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(result.Text())!;

            Assert.Equal(1, model["userId"].GetInt32());
            Assert.Equal(1, model["id"].GetInt32());
            Assert.Equal("delectus aut autem", model["title"].GetString());
            Assert.False(model["completed"].GetBoolean());
        }

        [Fact]
        public async void FetchGenericFunctionShouldWorkProperly()
        {

        }
    }
}