using DotnetFetch.Models;

namespace DotnetFetch;

/// <summary>
/// Represents an exception thrown when a CORS error is detected.
/// </summary>
/// <remarks>
/// This class currently derives from <see cref="FetchCorsException"/> and
/// subsequently <see cref="ArgumentException"/>. In the future, this derivation will be removed
/// in favor of a direct derivation from <see cref="Exception"/>.
/// </remarks>
public class CorsException

#pragma warning disable CS0618 // Type or member is obsolete

    // Justification: This derivation is in place to preserve backwards compatibility with the FetchCorsException class.
    : FetchCorsException

#pragma warning restore CS0618 // Type or member is obsolete

{
    /// <summary>
    /// Creates a new <see cref="CorsException"/> from the specified URL with a default message.
    /// </summary>
    /// <param name="url">The URL that caused the exception.</param>
    public CorsException(string url)
        : base("A CORS exception has occurred, check your request options.") =>
        Url = url;
    
    /// <summary>
    /// Creates a new <see cref="CorsException"/> from the specified URL with a custom message.
    /// </summary>
    /// <param name="url">The URL that caused the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    public CorsException(string url, string message)
        : base(message) =>
        Url = url;
    
    /// <summary>
    /// Gets or sets the URL that caused the exception.
    /// </summary>
    public string Url { get; }
}
