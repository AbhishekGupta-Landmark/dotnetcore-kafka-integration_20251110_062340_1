using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Confluent.Kafka;

namespace Api.Tests
{
    public class StartupTests
    {
        [Fact]
        public void ConfigureServices_ShouldAddMvc()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var services = new ServiceCollection();
            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(IServiceCollection));
        }

        [Fact]
        public void ConfigureServices_ShouldAddHostedService()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var services = new ServiceCollection();
            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(HostedServices.IHostedService));
        }

        [Fact]
        public void ConfigureServices_ShouldBindKafkaConfigs()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c.Bind("producer", It.IsAny<ProducerConfig>())).Verifiable();
            mockConfiguration.Setup(c => c.Bind("consumer", It.IsAny<ConsumerConfig>())).Verifiable();

            var services = new ServiceCollection();
            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            mockConfiguration.Verify(c => c.Bind("producer", It.IsAny<ProducerConfig>()), Times.Once);
            mockConfiguration.Verify(c => c.Bind("consumer", It.IsAny<ConsumerConfig>()), Times.Once);
        }

        [Fact]
        public void ConfigureServices_ShouldRegisterKafkaConfigs()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>();
            var services = new ServiceCollection();
            var startup = new Startup(mockConfiguration.Object);

            // Act
            startup.ConfigureServices(services);

            // Assert
            Assert.Contains(services, sd => sd.ServiceType == typeof(ProducerConfig) && sd.Lifetime == ServiceLifetime.Singleton);
            Assert.Contains(services, sd => sd.ServiceType == typeof(ConsumerConfig) && sd.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void Constructor_ShouldSetConfiguration()
        {
            // Arrange
            var mockConfiguration = new Mock<IConfiguration>().Object;

            // Act
            var startup = new Startup(mockConfiguration);

            // Assert
            Assert.Equal(mockConfiguration, startup.Configuration);
        }
    }
}