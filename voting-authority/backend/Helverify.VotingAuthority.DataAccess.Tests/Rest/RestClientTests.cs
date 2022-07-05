using System.Net;
using Helverify.VotingAuthority.DataAccess.Dto;
using Helverify.VotingAuthority.DataAccess.Rest;
using Moq;
using Moq.Protected;

namespace Helverify.VotingAuthority.DataAccess.Tests.Rest
{
    internal class RestClientTests
    {
        private const string C = "1f2a";
        private const string D = "ffa2";
        private readonly CipherTextDto _body = new() { C = C, D = D };

        [Test]
        public async Task TestCallReturn()
        {
            // arrange
            IHttpClientFactory httpClientFactory = SetUpHttpClientFactory(HttpStatusCode.OK);

            IRestClient restClient = new RestClient(httpClientFactory);

            // act
            CipherTextDto cipherTextDto = (await restClient.Call<CipherTextDto>(HttpMethod.Post, new Uri("http://localhost:9999"), _body))!;

            // assert
            Assert.That(cipherTextDto.C, Is.EqualTo(C));
            Assert.That(cipherTextDto.D, Is.EqualTo(D));
        }

        [Test]
        public void TestCallReturnError()
        {
            // arrange
            IHttpClientFactory httpClientFactory = SetUpHttpClientFactory(HttpStatusCode.InternalServerError);

            IRestClient restClient = new RestClient(httpClientFactory);

            // act, assert
            Assert.ThrowsAsync<Exception>(() => restClient.Call<CipherTextDto>(HttpMethod.Post, new Uri("http://localhost:9999"), _body));
        }

        [Test]
        public void TestCallNoReturn()
        {
            // arrange
            IHttpClientFactory httpClientFactory = SetUpHttpClientFactory(HttpStatusCode.OK);

            IRestClient restClient = new RestClient(httpClientFactory);

            // act
            Assert.DoesNotThrowAsync(() => restClient.Call(HttpMethod.Post, new Uri("http://localhost:9999"), _body));
        }

        [Test]
        public void TestCallNoReturnError()
        {
            // arrange
            IHttpClientFactory httpClientFactory = SetUpHttpClientFactory(HttpStatusCode.BadRequest);

            IRestClient restClient = new RestClient(httpClientFactory);

            // act
            Assert.ThrowsAsync<Exception>(() => restClient.Call(HttpMethod.Post, new Uri("http://localhost:9999"), _body));
        }

        private IHttpClientFactory SetUpHttpClientFactory(HttpStatusCode statusCode)
        {
            Mock<HttpMessageHandler> messageHandler = new Mock<HttpMessageHandler>();

            // according to: https://edi.wang/post/2021/5/11/aspnet-core-unit-test-how-to-mock-httpclientgetstringasync
            messageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("{ \"c\": \"" + C + "\", \"d\": \"" + D + "\"}"),
                    StatusCode = statusCode
                });
            HttpClient httpClient = new HttpClient(messageHandler.Object);

            Mock<IHttpClientFactory> httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return httpClientFactory.Object;
        }
    }
}
