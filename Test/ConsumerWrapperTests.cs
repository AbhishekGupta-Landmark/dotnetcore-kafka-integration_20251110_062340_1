using System;
using Xunit;
using Confluent.Kafka;
using Moq;

namespace Api.Tests
{
    public class ConsumerWrapperTests
    {
        [Fact]
        public void Constructor_ValidConfig_ShouldInitializeConsumer()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);

            // Assert
            Assert.NotNull(consumerWrapper);
        }

        [Fact]
        public void ReadMessage_ShouldReturnMessageValue()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var mockConsumer = new Mock<IConsumer<string, string>>();
            var mockConsumeResult = new ConsumeResult<string, string>
            {
                Value = "Test Message"
            };

            mockConsumer.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                .Returns(mockConsumeResult);

            // Act
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);
            var result = consumerWrapper.readMessage();

            // Assert
            Assert.Equal("Test Message", result);
        }

        [Fact]
        public void Constructor_NullConfig_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ConsumerWrapper(null, "test-topic"));
        }

        [Fact]
        public void Constructor_EmptyTopicName_ShouldThrowArgumentException()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ConsumerWrapper(mockConfig, string.Empty));
        }

        [Fact]
        public void ReadMessage_NoMessageAvailable_ShouldHandleAppropriately()
        {
            // Arrange
            var mockConfig = new ConsumerConfig { GroupId = "test-group" };
            var topicName = "test-topic";
            var mockConsumer = new Mock<IConsumer<string, string>>();

            mockConsumer.Setup(c => c.Consume(It.IsAny<CancellationToken>()))
                .Returns((ConsumeResult<string, string>)null);

            // Act & Assert
            var consumerWrapper = new ConsumerWrapper(mockConfig, topicName);
            Assert.Throws<InvalidOperationException>(() => consumerWrapper.readMessage());
        }
    }
}