using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Company.Function;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Mvc;

namespace tests
{
    public class FunctionTests
    {
        private readonly ILogger<GetResumeCounter> _logger;

        public FunctionTests()
        {
            // Create a logger instance
            _logger = new LoggerFactory().CreateLogger<GetResumeCounter>();
        }

        [Fact]
        public async Task GetResumeCounter_ShouldReturnValidResponse()
        {
            // Arrange
            var function = new GetResumeCounter(_logger);
            var request = TestFactory.CreateHttpRequest();

            // Act
            var result = await function.Run(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
