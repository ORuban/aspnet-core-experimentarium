using System;
using System.Net.Http;
using System.Threading.Tasks;
using Experimentarium.AspNetCore.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Experimentarium.AspNetCore.IntegrationTests
{
    public class HelloMiddleware
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public HelloMiddleware()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ReturnHelloWorld()
        {
            // Act
            var response = await _client.GetAsync("/middleware/hello");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("Hello World!", responseString);
        }
    }
}
