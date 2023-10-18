using Microsoft.Win32;
using Newtonsoft.Json;
using StorageLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace StorageLib
{
    /// <summary>
    /// Storage handling for keyword-awnser pairs
    /// </summary>
    public class Storage : INotifyPropertyChanged
    {

        /// <summary>
        /// Keyword - Awnser pairs
        /// </summary>
        public List<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Count));
            }
        }
        private List<Message> _messages = new List<Message>();

        /// <summary>
        /// Count accessor
        /// </summary>
        public int Count => _count(Messages);

        public Storage() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Simplify the usage of OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Receive a message by keyword
        /// </summary>
        /// <param name="keyword">The keyword identifying a message</param>
        /// <param name="fallback">The default message for non-found keywords</param>
        /// <returns>The found Message</returns>
        public Message? GetMessage(string keyword, Message? tree = null)
        {
            // Filter the array for the keyword
            // Will use the Observable collection if no Message passed, the tree else.
            Message[] messages = (tree?.Children.ToImmutableArray() ?? Messages.ToImmutableArray()).Where(m =>
            {
                switch (m.Type)
                {
                    case KeywordDetection.MatchFullCaseSensitive:
                        return m.Keyword.Trim() == keyword.Trim();
                    case KeywordDetection.MatchPartialCaseSensitive:
                        return keyword.Trim().Contains(m.Keyword.Trim());
                    case KeywordDetection.MatchFull:
                        return m.Keyword.ToLower().Trim() == keyword.ToLower().Trim();
                    // MatchPartial is the default
                    default:
                        return keyword.ToLower().Trim().Contains(m.Keyword.ToLower().Trim());
                }
            }).ToArray();
            // Return the default since no match for the keyword was found
            if (messages.Length == 0) return null;
            return messages.First();
        }

        /// <summary>
        /// Get the top level keywords
        /// </summary>
        /// <returns>The keywords existing</returns>
        public string[] GetTopLevelKeywords()
        {
            return Messages.Select(m => m.Keyword).ToArray();
        }

        /// <summary>
        /// Allow a file path to be prodived for Import, it will be converted to a stream and use the Stream Import
        /// </summary>
        /// <param name="filePath">Path to the file to Import</param>
        public void Import(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                Import(stream);
            }
        }

        /// <summary>
        /// Import from a stream to allow compatibility with embedded ressources
        /// </summary>
        /// <param name="stream">The Stream to read</param>
        /// <exception cref="ImportFormatException">Thrown on invalid Data</exception>
        public void Import(Stream stream)
        {
            using (StreamReader fileStream = new StreamReader(stream))
            using (JsonTextReader reader = new JsonTextReader(fileStream))
            {
                JsonSerializer serializer = new JsonSerializer();
                try
                {
                    List<Message>? importedMessages = serializer.Deserialize<List<Message>>(reader);
                    if (importedMessages == null || !importedMessages.Any())
                    {
                        throw new ImportFormatException();
                    }

                    foreach (var message in importedMessages)
                    {
                        if (!message.Validate())
                        {
                            throw new ImportFormatException(
                                $"Jede nachricht muss ein Schlüsselwort und eine Antwort enthalten.\n\nFehlerhalft:" +
                                $"\nSchlüsselwort: {(message.Keyword.Length > 0 ? message.Keyword : "????????????????")}" +
                                $"\nAntwort: {(message.Answer.Length > 0 ? message.Answer : "????????????????")}");
                        }
                    }

                    Messages = importedMessages;
                }
                catch (Exception ex) when (ex is JsonReaderException || ex is JsonSerializationException)
                {
                    throw new InvalidDataException("Provided file does not contain valid JSON.");
                }
            }
        }

        /// <summary>
        /// Count the current Keywords in the Storage
        /// </summary>
        /// <param name="messages">The message object to count the Keywords for</param>
        /// <returns>The count of all keywords</returns>
        private int _count(List<Message> messages)
        {
            int count = 0;
            foreach (var message in messages)
            {
                count++;
                if (message.Children != null)
                {
                    count += _count(message.Children);
                }
            }
            return count;
        }
    }
}