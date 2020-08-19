using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XCore.Services.Audit.Test.Integration
{
    public class AuditControllerTest
    {
        //[Fact]
        [Theory]
        [InlineData("POST", "0.1")]
        public async Task AuditGetTestAsync(string method, string version = null)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), $"/api/{version}/Audit/");

            // Act
            var response = await AuditClient.HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
