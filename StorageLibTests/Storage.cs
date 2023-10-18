namespace StorageLib.Tests
{
    public class StorageTests
    {
        [Fact]
        public void Constructor_InitializesCorrectly()
        {
            var storage = new Storage();

            Assert.NotNull(storage.Messages);
            Assert.Equal(0, storage.Count);
        }

        [Fact]
        public void PropertyChangedEvent_RaisedWhenMessagesChanged()
        {
            var storage = new Storage();
            bool eventRaised = false;
            storage.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Messages")
                {
                    eventRaised = true;
                }
            };

            storage.Messages = new List<Message>();

            Assert.True(eventRaised);
        }

        [Theory]
        [InlineData("key1", "answer1")]
        [InlineData("key2", "answer2")]
        public void GetMessage_ReturnsCorrectMessage(string keyword, string expectedAnswer)
        {
            var storage = new Storage
            {
                Messages = new List<Message>
                {
                    new Message { Keyword = "key1", Answer = "answer1", Type = KeywordDetection.MatchFull },
                    new Message { Keyword = "key2", Answer = "answer2", Type = KeywordDetection.MatchFull }
                }
            };

            var message = storage.GetMessage(keyword);

            Assert.Equal(expectedAnswer, message?.Answer);
        }

        [Fact]
        public void Count_ReturnsCorrectCount()
        {
            var storage = new Storage
            {
                Messages = new List<Message>
                {
                    new Message { Keyword = "key1", Answer = "answer1", Children = new List<Message> { new Message { Keyword = "childKey1", Answer = "childAnswer1" } } },
                    new Message { Keyword = "key2", Answer = "answer2" }
                }
            };

            Assert.Equal(3, storage.Count);
        }

        [Fact]
        public void GetTopLevelKeywords_ReturnsCorrectKeywords()
        {
            var storage = new Storage
            {
                Messages = new List<Message>
                {
                    new Message { Keyword = "key1", Answer = "answer1" },
                    new Message { Keyword = "key2", Answer = "answer2" }
                }
            };

            var keywords = storage.GetTopLevelKeywords();

            Assert.Equal(2, keywords.Length);
            Assert.Contains("key1", keywords);
            Assert.Contains("key2", keywords);
        }

        [Fact]
        public void Import_ThrowsExceptionForInvalidData()
        {
            var storage = new Storage();
            var invalidJson = "{ invalidJson: true }";

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(invalidJson)))
            {
                Assert.Throws<InvalidDataException>(() => storage.Import(stream));
            }
        }
    }
}
