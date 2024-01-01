using DotnetFetch.Models;

namespace DotnetFetch.Tests.Obsolete;

public class InvalidCharsetExceptionTests
{
    [Fact]
    public void Catch_FetchInvalidCharsetException_HandlesInvalidCharsetException() =>
        _ = Assert.Throws<InvalidCharsetException>(TestAction);
    
    private void TestAction()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        
        try
        {
            throw new InvalidCharsetException("utf-8");
        }
        catch (FetchInvalidCharsetException e)
        {
            _ = Assert.IsType<InvalidCharsetException>(e);
            throw;
        }
        
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
