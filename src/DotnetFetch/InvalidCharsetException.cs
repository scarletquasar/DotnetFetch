using DotnetFetch.Models;

namespace DotnetFetch;

/// <summary>
/// Represents an exception thrown when an invalid charset is detected.
/// </summary>
/// <remarks>
/// This class currently derives from <see cref="FetchInvalidCharsetException"/> and
/// subsequently <see cref="ArgumentException"/>. In the future, this derivation will be removed
/// in favor of a direct derivation from <see cref="Exception"/>.
/// </remarks>
public class InvalidCharsetException

#pragma warning disable CS0618 // Type or member is obsolete

    // Justification: This derivation is in place to preserve backwards compatibility with the FetchInvalidCharsetException class.
    : FetchInvalidCharsetException

#pragma warning restore CS0618 // Type or member is obsolete

{
    /// <summary>
    /// Creates a new <see cref="InvalidCharsetException"/> from the specified charset with a default message.
    /// </summary>
    /// <param name="charset">The charset that caused the exception.</param>
    public InvalidCharsetException(string charset)
        : base(charset, message: "Invalid charset detected.") =>
        Charset = charset;
    
    /// <summary>
    /// Creates a new <see cref="InvalidCharsetException"/> from the specified charset with a custom message.
    /// </summary>
    /// <param name="charset">The charset that caused the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    public InvalidCharsetException(string charset, string message)
        : base(charset, message) =>
        Charset = charset;

    /// <summary>
    /// Gets the charset that caused the exception.
    /// </summary>
    public string Charset { get; }
}