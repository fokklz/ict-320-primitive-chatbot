using PrimitiveChatBot.Common;
using System.Linq;
using Xunit;

namespace PrimitiveChatBot.Tests
{
    public class BotEngineTests
    {
        private BotEngine _botEngine;

        public BotEngineTests()
        {
            _botEngine = new BotEngine();
        }

        [Fact]
        public void BotEngine_Initialization_CorrectlySetsProperties()
        {
            // Assert
            Assert.True(_botEngine.HasNext);
            Assert.NotNull(_botEngine.NextKeywords);
            Assert.Null(_botEngine.Conversation);
        }

        [Fact]
        public void GetAnswer_WithTopLevelKeyword_ReturnsExpectedAnswer()
        {
            // Arrange
            var keyword = _botEngine.NextKeywords.FirstOrDefault(); // Assuming the storage has some top-level keywords.
            if (keyword == null) return; // Exit if no keywords.

            // Act
            var answer = _botEngine.GetAnswer(keyword);

            // Assert
            Assert.NotNull(answer);
            Assert.NotEqual("Sieht so aus, als hätte ich diesen Wissensbyte in meiner anderen Hose gelassen.", answer);
        }

        [Fact]
        public void GetAnswer_WithInvalidKeyword_ReturnsDefaultAnswer()
        {
            // Act
            var answer = _botEngine.GetAnswer("NonExistentKeyword");

            // Assert
            Assert.Equal("Sieht so aus, als hätte ich diesen Wissensbyte in meiner anderen Hose gelassen.", answer);
        }

        [Fact]
        public void Reset_AfterGettingAnswer_ResetsBotEngineState()
        {
            // Arrange
            var keyword = _botEngine.NextKeywords.FirstOrDefault();
            if (keyword == null) return;
            _botEngine.GetAnswer(keyword);

            // Act
            _botEngine.Reset();

            // Assert
            Assert.True(_botEngine.HasNext);
            Assert.Null(_botEngine.Conversation);
            Assert.NotNull(_botEngine.NextKeywords);
        }

        [Fact]
        public void PropertyChangedEvent_RaisesCorrectly()
        {
            string changedPropertyName = "";
            _botEngine.PropertyChanged += (s, e) =>
            {
                changedPropertyName = e.PropertyName;
            };

            // Act
            _botEngine.HasNext = false;

            // Assert
            Assert.Equal(nameof(_botEngine.HasNext), changedPropertyName);
        }
    }
}
