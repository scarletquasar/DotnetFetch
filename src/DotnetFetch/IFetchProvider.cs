using System.Text.Json.Nodes;
using DotnetFetch.Models;

namespace DotnetFetch;

/// <summary>
/// Defines members for providing fetch operations.
/// </summary>
public interface IFetchProvider
{
    /// <summary>
    /// Gets or sets whether or not CORS exceptions should be thrown when a request fails due to CORS.
    /// </summary>
    bool EnableCorsExceptions { get; set; }
    
    /// <summary>
    /// Fetches a resource from an external provider over the network.
    /// </summary>
    /// <param name="resource">The URL of the resource to fetch.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
    /// <returns>A <see cref="Response"/> object containing the response from the request.</returns>
    Task<Response> Fetch(string resource, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches a resource from an external provider over the network.
    /// </summary>
    /// <param name="resource">The URL of the resource to fetch.</param>
    /// <param name="options">The options to use when fetching the resource.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
    /// <returns>A <see cref="Response"/> object containing the response from the request.</returns>
    Task<Response> Fetch(string resource, JsonObject options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches a resource from an external provider over the network.
    /// </summary>
    /// <param name="resource">The URL of the resource to fetch.</param>
    /// <param name="options">The options to use when fetching the resource.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
    Task<Response> Fetch(string resource, FetchOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches a resource from an external provider over the network.
    /// </summary>
    /// <param name="resource">The URL of the resource to fetch.</param>
    /// <param name="options">The options to use when fetching the resource.</param>
    /// <param name="client">An <see cref="HttpClient"/> to use for the request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
    /// <returns>A <see cref="Response"/> object containing the response from the request.</returns>
    Task<Response> Fetch(string resource, JsonObject options, HttpClient client, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Fetches a resource from an external provider over the network.
    /// </summary>
    /// <param name="resource">The URL of the resource to fetch.</param>
    /// <param name="options">The options to use when fetching the resource.</param>
    /// <param name="client">An <see cref="HttpClient"/> to use for the request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the request.</param>
    /// <returns>A <see cref="Response"/> object containing the response from the request.</returns>
    Task<Response> Fetch(string resource, FetchOptions options, HttpClient client, CancellationToken cancellationToken = default);
}
