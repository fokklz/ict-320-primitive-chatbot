using StorageLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveChatBot.Common
{
    /// <summary>
    /// Represents a primitive BotEngine to facilitate the retrieval of messages from a Storage
    /// </summary>
    public class BotEngine: INotifyPropertyChanged
    {
        public bool HasNext { 
            get => _hasNext;
            set { 
                _hasNext = value;
                OnPropertyChanged();
            } 
        }
        private bool _hasNext = true;

        public bool IsAdmin => App.IsAdmin;

        /// <summary>
        /// The current conservation location
        /// </summary>
        public Message? Conversation
        {
            get => _message;
            set
            {
                _message = value;
                if (value != null)
                {
                    if (value.HasNextKeywords())
                    {
                        NextKeywords = value.GetNextKeywords();
                    }
                    else
                    {
                        HasNext = false;
                        NextKeywords = new string[0] {};
                    }
                }
                else
                {
                    NextKeywords = Storage.GetTopLevelKeywords();
                }
                OnPropertyChanged();
            }
        }
        private Message? _message;


        /// <summary>
        /// Current possible things to write
        /// </summary>
        public string[] NextKeywords
        {
            get => _nextKeywords;
            set
            {
                _nextKeywords = value;
                OnPropertyChanged();
            }
        }
        private string[] _nextKeywords = new string[0]{};

        /// <summary>
        /// Storage for the messages
        /// </summary>
        public Storage Storage { get; set; } = new Storage();


        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Default ctor
        /// </summary>
        public BotEngine() {
            NextKeywords = Storage.GetTopLevelKeywords();
            Storage.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Storage.Messages))
                {
                    NextKeywords = Storage.GetTopLevelKeywords();
                }
            };
        }

        /// <summary>
        /// Simplify the usage of OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Wrapper for Storage.GetMessage to simplify access
        /// </summary>
        /// <param name="keyword">The keyword for the message</param>
        /// <returns>The found message or a default string</returns>
        public string GetAnswer(string keyword)
        {
            Message? target = Conversation == null 
                ? Storage.GetMessage(keyword) 
                : Storage.GetMessage(keyword, Conversation);

            Conversation = target;
            if (target != null)
            {
                // Update the state for the conversation
                return target.Answer;
            }
            return "Sieht so aus, als hätte ich diesen Wissensbyte in meiner anderen Hose gelassen.";
        }

        /// <summary>
        /// Reset the BotEngine to start a new conversation
        /// </summary>
        public void Reset()
        {
            NextKeywords = Storage.GetTopLevelKeywords();
            HasNext = true;
            Conversation = null;
        }
    }
}
