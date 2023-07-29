namespace DotnetFetch.Models
{
    /// <summary>
    /// Represents an exception thrown when an invalid charset is detected.
    /// </summary>
    /// <remarks>This exception is a form of <see cref="ArgumentException"/>.</remarks>
    /// <seealso cref="ArgumentException"/>
    public class FetchInvalidCharsetException : ArgumentException
    {
        /// <summary>
        /// Creates a new <see cref="FetchInvalidCharsetException"/> with a default message.
        /// </summary>
        /// <param name="charset">The invalid charset that was detected.</param>
        public FetchInvalidCharsetException(string charset) : base($"Invalid charset: {charset}")
        { }
    }
}
