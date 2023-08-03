using System.Text.Json;

namespace DotnetFetch.Serialization;

/// <summary>
/// Represents the default <see cref="ISerializer"/> used by DotnetFetch.
/// </summary>
public class DefaultJsonSerializer
    : ISerializer
{
    /// <inheritdoc />
    /// <exception cref="ArgumentException">Thrown when <paramref name="content"/> is null, empty, or whitespace.</exception>
    public T? Deserialize<T>(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(message: "The content cannot be null or whitespace.", paramName: nameof(content));
        
        return JsonSerializer.Deserialize<T>(content);
    }

    /// <inheritdoc />
    public string Serialize<T>(T content) =>
        JsonSerializer.Serialize(content);
}