using System;
using Xunit;
using Confluent.Kafka;
using Moq;
using System.Threading.Tasks;

namespace Api.Tests
{
    public class ProducerWrapperTests
    {
        [Fact]
        public async Task WriteMessage_ValidMessage_ShouldProduceSuccessfully()
        {
            // Arrange
            var mockConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var producerWrapper = new ProducerWrapper(mockConfig, "test-topic");

            // Act
            await producerWrapper.writeMessage("test message");

            // Assert
            // Verify message was sent (may require mocking Kafka producer)
        }

        [Fact]
        public void Constructor_ValidConfiguration_ShouldInitializeProducer()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act
            var producerWrapper = new ProducerWrapper(config, "test-topic");

            // Assert
            Assert.NotNull(producerWrapper);
        }

        [Fact]
        public async Task WriteMessage_NullMessage_ShouldHandleGracefully()
        {
            // Arrange
            var mockConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            var producerWrapper = new ProducerWrapper(mockConfig, "test-topic");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => producerWrapper.writeMessage(null));
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProducerWrapper(null, "test-topic"));
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentException()
        {
            // Arrange
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ProducerWrapper(config, string.Empty));
        }
    }
}