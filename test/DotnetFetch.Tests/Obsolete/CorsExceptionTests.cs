using DotnetFetch.Models;

namespace DotnetFetch.Tests.Obsolete;

using Xunit;
using System;

public class CorsExceptionTests
{
    [Fact]
    public void Catch_FetchCorsException_HandlesCorsException() =>
        _ = Assert.Throws<CorsException>(TestAction);

    private void TestAction()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        
        try
        {
            throw new CorsException("https://example.com");
        }
        catch (FetchCorsException e)
        {
            _ = Assert.IsType<CorsException>(e);
            throw;
        }
        
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}