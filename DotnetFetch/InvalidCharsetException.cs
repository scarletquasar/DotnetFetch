namespace DotnetFetch;

/// <summary>
/// Represents an exception thrown when an invalid charset is detected.
/// </summary>
public class InvalidCharsetException
    : Exception
{
    /// <summary>
    /// Creates a new <see cref="InvalidCharsetException"/> from the specified charset with a default message.
    /// </summary>
    /// <param name="charset">The charset that caused the exception.</param>
    public InvalidCharsetException(string charset)
        : base(message: "Invalid charset detected.") =>
        Charset = charset;
    
    /// <summary>
    /// Creates a new <see cref="InvalidCharsetException"/> from the specified charset with a custom message.
    /// </summary>
    /// <param name="charset">The charset that caused the exception.</param>
    /// <param name="message">The message to use for the exception.</param>
    public InvalidCharsetException(string charset, string message)
        : base(message) =>
        Charset = charset;

    /// <summary>
    /// Gets the charset that caused the exception.
    /// </summary>
    public string Charset { get; }
}