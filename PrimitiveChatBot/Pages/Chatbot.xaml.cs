using PrimitiveChatBot.Common;
using PrimitiveChatBot.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimitiveChatBot.Pages
{
    /// <summary>
    /// Code-Behind for Chatbot.xaml
    /// Handles the core interaction logic for the chatbot
    /// </summary>
    public partial class Chatbot : Page
    {

        /// <summary>
        /// Default Ctor called by WPF on page load
        /// </summary>
        public Chatbot()
        {
            InitializeComponent();
            DataContext = App.BotEngine;

            App.BotEngine.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "HasNext")
                {
                    if (!App.BotEngine.HasNext)
                    {
                        SendButton.Content = "Reset";
                    }
                    else
                    {
                        SendButton.Content = "Send";
                    }
                }
            };
        }

        /// <summary>
        /// Append a message to the ChatStackPanel
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <param name="user">True if by User</param>
        private void addMessage(string message, bool user = true)
        {
            // Add the grid to the stack panel
            ChatStackPanel.Children.Add(new MessageEntry()
            {
                User = user ? "You" : "Bot",
                Message = message
            });
            // Scroll to the bottom
            ChatScrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// Process the input from the InputTextBox, clear it afterwards
        /// </summary>
        private void processInput(string? messageText)
        {
            if(messageText == null || messageText.Length == 0)
            {
                return;
            }
            addMessage(messageText);
            addMessage(App.BotEngine.GetAnswer(messageText), false);
            InputTextBox.Clear();
        }

        private void processInput()
        {
            processInput(InputTextBox.Text);
        }

        /// <summary>
        /// Handle the SendButton click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.BotEngine.HasNext)
            {
                processInput();
            }
            else
            {
                App.BotEngine.Reset();
            }
        }

        /// <summary>
        /// Handle the InputTextBox keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                processInput();
            }
        }

        /// <summary>
        /// Handle the ManageMessages click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageMessages_Click(object sender, RoutedEventArgs e)
        {
            ViewHandler.NavigateTo("Builder");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            processInput((sender as Button)?.Content as string);
        }
    }
}
