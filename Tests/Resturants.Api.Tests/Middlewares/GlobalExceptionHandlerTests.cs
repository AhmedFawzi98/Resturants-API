using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Resturants.Api.Middlewares;

namespace Resturants.Api.Tests.Middlewares;

[TestFixture]
public class GlobalExceptionHandlerTests
{
    private readonly Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
    private GlobalExceptionHandler _cut;

    public GlobalExceptionHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        _cut = new GlobalExceptionHandler(_loggerMock.Object);
    }

    [Test]
    public async Task TryHandleAsyncTest()
    {
        var context = new DefaultHttpContext();

        var exception = new Exception();
            
        bool result = await _cut.TryHandleAsync(context, exception, CancellationToken.None);

        Assert.That(result, Is.True);

        Assert.That(context.Response.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(context.Response.ContentType, Is.EqualTo("text/plain"));

    }
}