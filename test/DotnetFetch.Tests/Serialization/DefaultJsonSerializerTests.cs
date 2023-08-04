using DotnetFetch.Serialization;

namespace DotnetFetch.Tests.Serialization;

public class DefaultJsonSerializerTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Deserialize_NullEmptyOrWhitespaceContent_ThrowsArgumentException(string content)
    {
        // Arrange
        DefaultJsonSerializer serializer = new();
        
        // Act
        void TestAction() => serializer.Deserialize<object>(content);
        
        // Assert
        Assert.Throws<ArgumentException>(TestAction);
    }
    
    [Fact]
    public void Deserialize_ValidContent_ReturnsDeserializedObject()
    {
        // Arrange
        DefaultJsonSerializer serializer = new();
        string content = "{\"foo\":\"bar\"}";
        
        // Act
        object? result = serializer.Deserialize<object>(content);
        
        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public void Serialize_ValidObject_ReturnsSerializedString()
    {
        // Arrange
        DefaultJsonSerializer serializer = new();
        object content = new { foo = "bar" };
        
        // Act
        string result = serializer.Serialize(content);
        
        // Assert
        Assert.NotNull(result);
    }
}