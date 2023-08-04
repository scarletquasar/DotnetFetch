using System.Text.Json.Nodes;
using DotnetFetch.Models;

namespace DotnetFetch;

public class BasicFetchProvider
    : IFetchProvider
{
    /// <inheritdoc />
    public bool EnableCorsExceptions { get; set; }
    
    /// <inheritdoc />
    public Task<Response> Fetch(string resource, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Response> Fetch(string resource, JsonObject options, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Response> Fetch(string resource, FetchOptions options, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Response> Fetch(string resource, JsonObject options, HttpClient client, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<Response> Fetch(string resource, FetchOptions options, HttpClient client, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}