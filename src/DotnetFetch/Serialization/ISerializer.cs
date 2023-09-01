namespace DotnetFetch.Serialization;

/// <summary>
/// Defines methods for serializing and deserializing objects.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Deserializes a specified string into an object.
    /// </summary>
    /// <param name="content">The string to deserialize.</param>
    /// <typeparam name="T">The type of the object to deserialize into.</typeparam>
    /// <returns>The deserialized object.</returns>
    T? Deserialize<T>(string content);
    
    /// <summary>
    /// Serializes a specified object into a string.
    /// </summary>
    /// <param name="content">The object to serialize.</param>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <returns>The serialized string.</returns>
    string Serialize<T>(T content);
}
