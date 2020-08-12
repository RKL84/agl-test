using Agl.Core.Configuration;
using Agl.Core.Infrastructure.Services;
using Agl.Core.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Agl.UnitTest
{
    [TestClass]
    public class PeopleServiceTest
    {
        [TestMethod]
        public async Task TestService_OnSuccessResponseFromServer_ShouldReturnResult()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var mockResponse = new List<Person>();
            var person = new Person();
            person.Name = "Bob";
            person.Gender = "Male";
            person.Age = 2;
            person.PetCollection = new List<Pet>();
            person.PetCollection.Add(new Pet() { Name = "Garfield", Type = "Cat" });
            mockResponse.Add(person);

            var mockResponseString = Newtonsoft.Json.JsonConvert.SerializeObject(mockResponse);
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var response = new HttpResponseMessage();
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Content = new StringContent(mockResponseString);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });

            var logger = Mock.Of<ILogger<PeopleService>>();
            var httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("http://test/");
            var appSettings = new AppSettings();
            appSettings.PeopleServiceClientConfig = new PeopleServiceClientConfig() { Url = "http://test/", Endpoint = "endpoint" };
            var peopleService = new PeopleService(httpClient, appSettings, logger);
            var result = await peopleService.FetchAllAsync();
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task TestService_OnErrorFromServer_ShouldThrowException()
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ThrowsAsync(new Exception("Error"));

            var logger = Mock.Of<ILogger<PeopleService>>();
            var httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("http://test/");
            var appSettings = new AppSettings();
            appSettings.PeopleServiceClientConfig = new PeopleServiceClientConfig() { Url = "http://test/", Endpoint = "endpoint" };
            var peopleService = new PeopleService(httpClient, appSettings, logger);
            try
            {
                await peopleService.FetchAllAsync();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Error", ex.Message);
            }
        }
    }
}
