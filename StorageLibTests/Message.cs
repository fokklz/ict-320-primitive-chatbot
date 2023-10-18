using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Xunit;
using StorageLib;

namespace StorageLib.Tests
{
    public class MessageTests
    {
        [Fact]
        public void Constructor_SetsExpectedProperties()
        {
            var message = new Message("hello", "hi");

            Assert.Equal("hello", message.Keyword);
            Assert.Equal("hi", message.Answer);
            Assert.Equal("hello_tree", message.Slug);
            Assert.Equal(KeywordDetection.MatchPartial, message.Type);
        }

        [Fact]
        public void ConstructorWithDetectionType_SetsExpectedProperties()
        {
            var message = new Message("hello", "hi", KeywordDetection.MatchFull);

            Assert.Equal("hello", message.Keyword);
            Assert.Equal("hi", message.Answer);
            Assert.Equal("hello_tree", message.Slug);
            Assert.Equal(KeywordDetection.MatchFull, message.Type);
        }

        [Fact]
        public void FindMessageBySlug_FindsCorrectMessage()
        {
            var parent = new Message("parent", "parent_answer");
            var child = new Message("child", "child_answer");
            parent.Children.Add(child);

            Assert.Equal(child, parent.FindMessageBySlug("child_tree"));
        }

        [Fact]
        public void FindMessageBySlug_ReturnsNullForMissingSlug()
        {
            var parent = new Message("parent", "parent_answer");

            Assert.Null(parent.FindMessageBySlug("missing_tree"));
        }

        [Fact]
        public void HasNextKeywords_ReturnsTrueWithChildren()
        {
            var parent = new Message("parent", "parent_answer");
            var child = new Message("child", "child_answer");
            parent.Children.Add(child);

            Assert.True(parent.HasNextKeywords());
        }

        [Fact]
        public void HasNextKeywords_ReturnsFalseWithoutChildren()
        {
            var parent = new Message("parent", "parent_answer");

            Assert.False(parent.HasNextKeywords());
        }

        [Fact]
        public void GetNextKeywords_ReturnsCorrectKeywords()
        {
            var parent = new Message("parent", "parent_answer");
            var child1 = new Message("child1", "child1_answer");
            var child2 = new Message("child2", "child2_answer");
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            var expected = new string[] { "child1", "child2" };
            Assert.Equal(expected, parent.GetNextKeywords());
        }

        [Fact]
        public void Validate_ReturnsFalseForEmptyFields()
        {
            var message = new Message("", "");

            Assert.False(message.Validate());
        }

        [Fact]
        public void Validate_ReturnsTrueForValidMessage()
        {
            var message = new Message("hello", "hi");

            Assert.True(message.Validate());
        }

        [Fact]
        public void Validate_ReturnsFalseForInvalidChild()
        {
            var parent = new Message("parent", "parent_answer");
            var child = new Message("", "");
            parent.Children.Add(child);

            Assert.False(parent.Validate());
        }
    }
}

