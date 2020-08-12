using Agl.Client;
using Agl.Core.Infrastructure.Services;
using Agl.Core.Model;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agl.UnitTest
{
    [TestClass]
    public class PeopleInfoConsoleWriterTest
    {

        [TestMethod]
        public async Task TestService_OnSuccessResponseFromPeopleServiceClient_ShouldDisplayResult()
        {
            var mockResponse = new List<Person>();
            var person = new Person();
            person.Name = "Bob";
            person.Gender = "Male";
            person.Age = 2;
            person.PetCollection = new List<Pet>();
            person.PetCollection.Add(new Pet() { Name = "Garfield", Type = "Cat" });
            mockResponse.Add(person);

            var logger = Mock.Of<ILogger<PeopleInfoConsoleWriter>>();
            var peopleServiceMock = new Mock<IPeopleService>();
            peopleServiceMock.Setup(arg => arg.FetchAllAsync())
                .Returns(Task.FromResult(mockResponse.AsEnumerable()));

            var writer = new PeopleInfoConsoleWriter(peopleServiceMock.Object, logger);
            await writer.Run();
        }

        [TestMethod]
        public async Task TestService_OnErrorFromPersonServiceClient_ShouldDisplayError()
        {
            var mockResponse = new List<Person>();
            var person = new Person();
            person.Name = "Bob";
            person.Gender = "Male";
            person.Age = 2;
            person.PetCollection = new List<Pet>();
            person.PetCollection.Add(new Pet() { Name = "Garfield", Type = "Cat" });
            mockResponse.Add(person);

            var logger = Mock.Of<ILogger<PeopleInfoConsoleWriter>>();
            var peopleServiceMock = new Mock<IPeopleService>();
            peopleServiceMock.Setup(arg => arg.FetchAllAsync()).ThrowsAsync(new Exception("Error"));

            try
            {
                var writer = new PeopleInfoConsoleWriter(peopleServiceMock.Object, logger);
                await writer.Run();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Error", ex.Message);
            }
        }
    }
}
