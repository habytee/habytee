using NUnit.Framework;
using habytee.Client.ViewModels;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using habytee.Client.Services;
using habytee.Interconnection.Models.Requests;
using System.Net.Http.Json;

namespace habytee.Tests.ViewModels
{
    public class AddHabitAlarmViewModelTests
    {
        [Test]
        public void SetAlarm_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var viewModel = new AddHabitAlarmViewModel(new ApiService(new HttpClient()));
            bool propertyChangedTriggered = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.SetAlarm))
                {
                    propertyChangedTriggered = true;
                }
            };

            // Act
            viewModel.SetAlarm = true;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);
        }

        [Test]
        public void AlarmTime_ShouldTriggerPropertyChanged()
        {
            // Arrange
            var viewModel = new AddHabitAlarmViewModel(new SomeDependency());
            bool propertyChangedTriggered = false;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(viewModel.AlarmTime))
                {
                    propertyChangedTriggered = true;
                }
            };

            // Act
            viewModel.AlarmTime = DateTime.Now;

            // Assert
            Assert.That(propertyChangedTriggered, Is.True);
        }
    }
}

namespace habytee.Tests.Services
{
    public class ApiServiceTests
    {
        private Mock<HttpClient> httpClientMock;
        private IApiService apiService;

        [SetUp]
        public void Setup()
        {
            httpClientMock = new Mock<HttpClient>();
            apiService = new ApiService(httpClientMock.Object);
        }

        [Test]
        public async Task SendHabitToApiAsync_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            var habitRequest = new CreateHabitRequest();
            httpClientMock.Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), habitRequest))
                          .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK));

            // Act
            var result = await apiService.SendHabitToApiAsync(habitRequest);

            // Assert
            Assert.That(result, Is.True);
        }
    }

    public class MessageServiceTests
    {
        [Test]
        public void SendMessage_ShouldTriggerOnMessageReceived()
        {
            // Arrange
            var messageService = new MessageService();
            string receivedMessage = null;
            messageService.OnMessageReceived += (message) => receivedMessage = message;

            // Act
            messageService.SendMessage("Test Message");

            // Assert
            Assert.That(receivedMessage, Is.EqualTo("Test Message"));
        }
    }
}
