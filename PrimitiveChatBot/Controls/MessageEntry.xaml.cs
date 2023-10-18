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

namespace PrimitiveChatBot.Controls
{
    /// <summary>
    /// Interaction logic for MessageEntry.xaml
    /// </summary>
    public partial class MessageEntry : UserControl
    {
        /// <summary>
        /// Local user name storage
        /// </summary>
        private string? _user;

        /// <summary>
        /// Setting the username will also set the background color
        /// If the user is "Bot", the background will be set to a darker color
        /// to simplify the distinction between user and bot messages
        /// </summary>
        public string User { 
            get {
                return _user;
            }
            set { 
                _user = value;
                if(value == "Bot")
                {
                    OddBackground.Fill = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
                }
            } 
        }

        /// <summary>
        /// The message to display
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Default ctor
        /// </summary>
        public MessageEntry()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
