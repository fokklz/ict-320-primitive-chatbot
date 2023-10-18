using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLib
{
    /// <summary>
    /// Possible Detection Types for a Message
    /// </summary>
    public enum KeywordDetection
    {
        MatchFull,
        MatchPartial,
        MatchFullCaseSensitive,
        MatchPartialCaseSensitive,
    }

    /// <summary>
    /// Represents a keyword-awnser pair
    /// </summary>
    public class Message
    {
        /// <summary>
        /// A unique identifier for the message
        /// Allows for conversation hopping
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The keyword to be contained/matched to display the matching anwser
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// The anwser for the keyword
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// The way the keyword should be matched
        /// </summary>
        public KeywordDetection Type { get; set; } = KeywordDetection.MatchPartial;

        /// <summary>
        /// Childrens for this conversation
        /// </summary>
        public List<Message> Children { get; set; } = new List<Message>();

        /// <summary>
        /// Basic constructor for a Message
        /// </summary>
        /// <param name="keyword">The keyword for a message to get triggered</param>
        /// <param name="anwser">The anwser the bot should give</param>
        public Message(string keyword, string anwser)
        {
            Keyword = keyword;
            Answer = anwser;
            Slug = $"{keyword}_tree";
        }

        /// <summary>
        /// Overflow constructor for a Message
        /// </summary>
        /// <param name="keyword">The keyword to match for the message to get triggered</param>
        /// <param name="anwser">The anwser the bot should give</param>
        /// <param name="type">The type of detection to use when trying to match a messge</param>
        public Message(string keyword, string anwser, KeywordDetection type) : this(keyword, anwser)
        {
            Type = type;
        }

        /// <summary>
        /// Basic empty constructor for JSON Seria
        /// </summary>
        public Message() : this(string.Empty, string.Empty) { }

        /// <summary>
        /// Find a message by its slug (will check all children as well)
        /// </summary>
        /// <param name="slug">The slug to search for</param>
        /// <returns>The message to return</returns>
        public Message? FindMessageBySlug(string slug)
        {
            if (Slug == slug) return this;

            foreach (var child in Children)
            {
                var found = child.FindMessageBySlug(slug);
                if (found != null) return found;
            }

            return null;
        }

        /// <summary>
        /// Check if the conversation tree has messages left
        /// </summary>
        /// <returns></returns>
        public bool HasNextKeywords()
        {
            return Children.Count > 0;
        }

        /// <summary>
        /// Get the next possible Keywords
        /// </summary>
        /// <returns></returns>
        public string[] GetNextKeywords()
        {
            return Children.Select(w => w.Keyword).ToArray();
        }

        /// <summary>
        /// Validate the message to be correct and usable by the program
        /// </summary>
        /// <returns>True if the message is valid</returns>
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Keyword) || string.IsNullOrWhiteSpace(Answer))
            {
                return false;
            }
            if (Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    if (!child.Validate())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
