namespace DotnetFetch.Models
{
    /// <summary>
    /// Represents an exception thrown when a CORS error is detected.
    /// </summary>
    /// <remarks>This exception is a form of <see cref="ArgumentException"/>.</remarks>
    /// <seealso cref="ArgumentException"/>
    [Obsolete(message: "This exception is obsolete and will be removed in a future version. Please use CorsException instead.")]
    public class FetchCorsException : ArgumentException
    {
        /// <summary>
        /// Creates a new <see cref="FetchCorsException"/> with a default message.
        /// </summary>
        public FetchCorsException()
            : base("A CORS exception has occurred, check your request options.") { }
    }
}
